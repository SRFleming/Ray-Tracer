using System;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an (infinite) plane in a scene.
    /// </summary>
    public class Sphere : SceneEntity
    {
        private Vector3 center;
        private double radius;
        private Material material;

        /// <summary>
        /// Construct a sphere given its center point and a radius.
        /// </summary>
        /// <param name="center">Center of the sphere</param>
        /// <param name="radius">Radius of the spher</param>
        /// <param name="material">Material assigned to the sphere</param>
        public Sphere(Vector3 center, double radius, Material material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        /// <summary>
        /// Determine if a ray intersects with the sphere, and if so, return hit data.
        /// </summary>
        /// <param name="ray">Ray to check</param>
        /// <returns>Hit data (or null if no intersection)</returns>
        public RayHit Intersect(Ray ray)
        {
            // Write your code here...
            double d0, d1; //q;
            Vector3 vposition = new Vector3(0,0,0); 
            Vector3 vnormal = new Vector3(0,0,0); 
            Vector3 L = this.center - ray.Origin;

            double tca = L.Dot(ray.Direction); 
            if (tca < 0) return null;
            double d2 = L.LengthSq() - tca * tca; 
            if (d2 > (this.radius * this.radius)) {return null; }
            double thc = Math.Sqrt(this.radius * this.radius - d2); 
            d0 = tca - thc; 
            d1 = tca + thc;
            /*double a = ray.Direction.LengthSq();
            double b = 2*(ray.Direction.Dot(L));
            double c = L.LengthSq() - this.radius * this.radius;

            double discriminant = -(b * b - 4 * a * c);

            if (discriminant > 0) {
                return null;
            } 
            else if (discriminant == 0) {
                d0 = d1 = - 0.5 * b / a; 
            }
            else {
                if (b > 0) {
                    q = -0.5 * (b + Math.Sqrt(discriminant));
                }
                else {
                    q = -0.5 * (b - Math.Sqrt(discriminant));
                }
                d0 = q / a;
                d1 = c / q;
            } */
            if (d0 <= 0) { 
                d0 = d1;  //if t0 is negative, let's use t1 instead 
                if (d0 <= 0) {
                    //Console.WriteLine("FAIL 1");
                    return null;  //both t0 and t1 are negative 
                }
            }
            vposition = ray.Origin + d0 * ray.Direction;
            //Console.WriteLine("PASS");
            Vector3 helper = vposition - this.center;
            vnormal = helper.Normalized();
            return new RayHit(vposition, vnormal, ray.Direction, this.material);
            
        }

        /// <summary>
        /// The material of the sphere.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
