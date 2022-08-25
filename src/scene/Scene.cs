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

        private Ray generateRay(int i, int j, int width, int height) {
            var aspectratio = width/ height;

            var scale = Math.Tan((Math.PI/6));
            var x = (2*(i + 0.5) / (float)width - 1) * aspectratio * scale;
            var y = (1 - 2*(j + 0.5f)/(float)height) * scale;

            Vector3 to3Dcoord = new Vector3(x,y,1);
            Vector3 rayDir = to3Dcoord.Normalized();
            Ray ray = new Ray(origin: new Vector3(0,0,0), rayDir);
            return ray;
        } 

        public void Render(Image outputImage)
        {
            // Begin writing your code here...
            Vector3 Origin = new Vector3(0,0,0);
            int i = 0, j = 0;
            //double[,] hitmatrix = new double[outputImage.Width, outputImage.Height];
            //Array.Clear(hitmatrix, 0, hitmatrix.Length);
            while(i < outputImage.Width) {
                while (j < outputImage.Height) {
                    Ray ray = generateRay(i, j, outputImage.Width, outputImage.Height);
                    foreach (SceneEntity entity in this.entities) {
                        RayHit hit = entity.Intersect(ray);
                        if (hit != null) {
                            //if (hitmatrix[i, j] == 0) {
                                outputImage.SetPixel(i, j, entity.Material.Color);
                               // hitmatrix[i, j] = hit.Position.Z;
                            //}
                            /*else if (hit.Position.Z > hitmatrix[i, j]){
                                outputImage.SetPixel(i, j, entity.Material.Color);
                                hitmatrix[i, j] = hit.Position.Z;
                            } */
                        }
                    }
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

