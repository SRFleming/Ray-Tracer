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

        private Vector3 normalizedcoords(Image outputImage, float x, float y) {
            return new Vector3(
            outputImage.Width * (x - 0.5f),
            outputImage.Height * (0.5f - y),
            1.0f); // Image plane is 1 unit from camera.
        } 

        private Vector3 CoordCenter(Image outputImage, float x, float y) {
            var NormX = (x + 0.5f) / outputImage.Height;
            var NormY = (0.5f + y) / outputImage.Width;
            return normalizedcoords(outputImage, NormX, NormY);
        }

        public void Render(Image outputImage)
        {
            // Begin writing your code here...
            Vector3 Origin = new Vector3(0,0,0);
            int i = 0, j = 0;
            while(i < outputImage.Width) {
                while (j < outputImage.Height) {
                    new Ray(Origin,CoordCenter(outputImage, i, j));
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

