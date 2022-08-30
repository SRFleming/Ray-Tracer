using System;
using System.Collections.Generic;

namespace RayTracer
{
    /// <summary>
    /// Class to represent a ray traced scene, including the objects,
    /// light sources, and associated rendering logic.
    /// </summary>
    public class Scene
    {
        private SceneOptions options;
        private ISet<SceneEntity> entities;
        private ISet<PointLight> lights;

        /// <summary>
        /// Construct a new scene with provided options.
        /// </summary>
        /// <param name="options">Options data</param>
        public Scene(SceneOptions options = new SceneOptions())
        {
            this.options = options;
            this.entities = new HashSet<SceneEntity>();
            this.lights = new HashSet<PointLight>();
        }

        /// <summary>
        /// Add an entity to the scene that should be rendered.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void AddEntity(SceneEntity entity)
        {
            this.entities.Add(entity);
        }

        /// <summary>
        /// Add a point light to the scene that should be computed.
        /// </summary>
        /// <param name="light">Light structure</param>
        public void AddPointLight(PointLight light)
        {
            this.lights.Add(light);
        }

        /// <summary>
        /// Render the scene to an output image. This is where the bulk
        /// of your ray tracing logic should go... though you may wish to
        /// break it down into multiple functions as it gets more complex!
        /// </summary>
        /// <param name="outputImage">Image to store render output</param>

        private Ray generateRay(int i, int j, int width, int height, double coordx, double coordy) {
            double aspectratio = width / height;

            double scale = Math.Tan((Math.PI/6));
            double x = (2*(i + coordx) / (float)width - 1) * aspectratio * scale;
            double y = (1 - 2*(j + coordy)/(float)height) * scale;

            Vector3 to3Dcoord = new Vector3(x,y,1);
            Vector3 rayDir = to3Dcoord.Normalized();
            Ray ray = new Ray(origin: new Vector3(0,0,0), rayDir);
            return ray;
        } 

        private int shadowtracer(Ray ray, double lengthtolight) {
            foreach (SceneEntity entity in this.entities) {
                RayHit hit = entity.Intersect(ray);
                if (hit != null && (hit.Position - ray.Origin).Length() < lengthtolight) {
                    return 1;
                }
            }
            return 0;
        }

        /*private Color EmissiveRadiation(Ray ray) {
            Color outputcolor = new Color(0,0,0);
            foreach (SceneEntity entity in this.entities) {
                if (entity.Material.Type == Material.MaterialType.Emissive) {
                    Vector3 L = (entity.Position - hit.Position).Normalized();
                    double lengthtolight = (entity. - hit.Position).Length();
                    Ray lightray = new Ray(hit.Position + 0.0001*L, L);
                    int hitObject = shadowtracer(lightray, lengthtolight);
                    if (hitObject == 0) {
                        Color addColor = entity.Material.Color * entity.Color * (L.Dot(hit.Normal));
                        outputColor = outputColor + addColor;
                    }
                }
            }
            return outputcolor;
        } */

        private Color Diffuse(Material Mat, RayHit hit, Ray ray) {
            Color outputColor = new Color(0,0,0);
            foreach (PointLight light in this.lights) {
                Vector3 L = (light.Position - hit.Position).Normalized();
                double lengthtolight = (light.Position - hit.Position).Length();
                Ray lightray = new Ray(hit.Position + 0.0001*L, L);
                int hitObject = shadowtracer(lightray, lengthtolight);
                if (hitObject == 0) {
                    Color addColor = Mat.Color * light.Color * (L.Dot(hit.Normal));
                    outputColor = outputColor + addColor;
                }
            }
            return outputColor;
        }

        private double fresnel(Vector3 I, Vector3 N, double ior) { 
            Vector3 n = N;
            double cosi = Math.Clamp(I.Dot(N), -1, 1); 
            double etai = 1, etat = ior; 
            double kr;
            if (cosi > 0) {
                double temp = etai;
                etai = etat;
                etat = temp;
            }
            // Compute sini using Snell's law
            double sint = etai / etat * Math.Sqrt(Math.Max(0f, 1 - cosi * cosi)); 
            // Total internal reflection
            if (sint >= 1) { 
                kr = 1; 
            } 
            else { 
                double cost = Math.Sqrt(Math.Max(0f, 1 - sint * sint));
                cosi = Math.Abs(cosi); 
                double Rs = ((etat * cosi) - (etai * cost)) / ((etat * cosi) + (etai * cost)); 
                double Rp = ((etai * cosi) - (etat * cost)) / ((etai * cosi) + (etat * cost)); 
                kr = (Rs * Rs + Rp * Rp) / 2; 
            } 
            // As a consequence of the conservation of energy, transmittance is given by:
            // kt = 1 - kr;
            return kr;
        }

        private Vector3 refract(Vector3 I, Vector3 N, double ior) 
        { 
            double cosi = Math.Clamp(I.Dot(N), -1, 1); 
            double etai = 1, etat = ior; 
            Vector3 n = N; 
            if (cosi < 0) { 
                cosi = -cosi; 
            } 
            else { 
                double temp = etai;
                etai = etat;
                etat = temp;
                n = -N; 
            }   
            double eta = etai / etat; 
            double k = 1 - eta * eta * (1 - cosi * cosi);
            if (k < 0) {
                return new Vector3(0,0,0);
            }
            else {
                return (eta * I + (eta * cosi - Math.Sqrt(k)) * n); 
            }
        } 

        private Color hitOptions(Ray incidentray, int depth, int i, int j) {
            Color color = new Color(0,0,0);
            Vector3 Origin = new Vector3(0,0,0);
            double distancetohit = 10000;
            foreach (SceneEntity entity in this.entities) {
                RayHit hit = entity.Intersect(incidentray);
                if (hit != null ) {
                    if (depth > 5) {
                        //Console.WriteLine("DEPTH MAX REACHED" + "i = " + i + "j = " + j);
                        return entity.Material.Color;
                    }
                    double temp = (hit.Position - incidentray.Origin).LengthSq();
                    if (temp < distancetohit) {
                        distancetohit = temp;
                        if (entity.Material.Type == Material.MaterialType.Diffuse) {
                            color = Diffuse(entity.Material, hit, incidentray);
                        }
                        else if (entity.Material.Type == Material.MaterialType.Reflective){
                            Vector3 reflectedvector = hit.Incident - 2 * hit.Normal * (hit.Incident.Dot(hit.Normal));
                            Ray reflectray = new Ray(hit.Position, reflectedvector);
                            color = hitOptions(reflectray, depth+1, i, j);
                        }
                        else if (entity.Material.Type == Material.MaterialType.Refractive) {
                            Color refractionColor = new Color(); 
                            double offset = 0.0001;
                            if (incidentray.Direction.Dot(hit.Normal) < 0) {
                                offset = -offset;
                            }
                            // compute fresnel
                            double kr = fresnel(incidentray.Direction, hit.Normal, entity.Material.RefractiveIndex); 
                            // compute refraction if it is not a case of total internal reflection
                            if (kr < 1) { 
                                Vector3 refractionDirection = refract(incidentray.Direction, hit.Normal, entity.Material.RefractiveIndex).Normalized(); 
                                Vector3 refractionRayOrig = hit.Position; 
                                Ray refractray = new Ray(refractionRayOrig + offset * hit.Normal, refractionDirection);
                                refractionColor = hitOptions(refractray, depth+1, i, j);
                            }
                            Vector3 reflectionDirection = (hit.Incident - 2 * hit.Normal * (hit.Incident.Dot(hit.Normal))).Normalized();
                            Vector3 reflectionRayOrig = hit.Position;
                            Ray reflectray = new Ray(reflectionRayOrig + offset * hit.Normal, reflectionDirection);
                            Color reflectionColor = hitOptions(reflectray, depth+1, i, j);
                                // mix the two
                            color = reflectionColor * kr + refractionColor * (1 - kr);
                        }
                        else if (entity.Material.Type == Material.MaterialType.Emissive) {
                            
                        }
                        else {
                            color = entity.Material.Color;
                        }
                    }
                }
            }
            return color;
        }

        public void Render(Image outputImage) {
            // Begin writing your code here...
            Vector3 Origin = new Vector3(0,0,0);
            int i = 0, j = 0;
            while(i < outputImage.Width) {
                while (j < outputImage.Height) {
                    Color outputColor = new Color();
                    Color averagedColor = new Color();
                    double multiplierx = 1; double multipliery = 1; double coordx; double coordy;
                    while (multiplierx <= options.AAMultiplier) {
                        while (multipliery <= options.AAMultiplier) {
                            coordx = multiplierx/((double)options.AAMultiplier+1);
                            coordy = multipliery/((double)options.AAMultiplier+1);
                            Ray ray = generateRay(i, j, outputImage.Width, outputImage.Height, coordx, coordy);
                            outputColor = hitOptions(ray, 0, i, j);
                            averagedColor += outputColor;
                            multipliery++;
                        }
                        multipliery = 1;
                        multiplierx++;
                    }
                    outputColor = averagedColor/(options.AAMultiplier*options.AAMultiplier);
                    outputImage.SetPixel(i, j, outputColor);
                    j++;
                }
                j = 0;
                i++;
            }

        }
    }
}

//cd C:\Users\Seb\Documents\Uni\2022\Graphics and Interaction\project-1-ray-tracer-SRFleming
//dotnet run -- -f tests/sample_scene_1.txt -o output.png

