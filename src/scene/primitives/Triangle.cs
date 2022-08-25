using System;

namespace RayTracer
{
    /// <summary>
    /// Class to represent a triangle in a scene represented by three vertices.
    /// </summary>
    public class Triangle : SceneEntity
    {
        private Vector3 v0, v1, v2;
        private Material material;

        /// <summary>
        /// Construct a triangle object given three vertices.
        /// </summary>
        /// <param name="v0">First vertex position</param>
        /// <param name="v1">Second vertex position</param>
        /// <param name="v2">Third vertex position</param>
        /// <param name="material">Material assigned to the triangle</param>
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Material material)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.material = material;
        }

        /// <summary>
        /// Determine if a ray intersects with the triangle, and if so, return hit data.
        /// </summary>
        /// <param name="ray">Ray to check</param>
        /// <returns>Hit data (or null if no intersection)</returns>
        public RayHit Intersect(Ray ray)
        {   
            Vector3 vposition = new Vector3(0,0,0); 
            Vector3 vincident = new Vector3(0,0,0);
            Vector3 v0v1 = v1 - v0;
            Vector3 v0v2 = v2 - v0;
            Vector3 vnormal = v0v1.Cross(v0v2);
            
            double determinant = - vnormal.Dot(ray.Direction);

            /*if (determinant < 1e-6) {
                //Console.WriteLine("FAIL 0");
                if (determinant > -1e-6) {
                    return null;
                }
            } */

            double d = - vnormal.Dot(v0);

            double t = -(vnormal.Dot(ray.Origin) + d) / determinant;

            if (t < 0) {
                //Console.WriteLine("FAIL 1");
                return null;
            }

            /*double invDet = 1 / determinant;

            /*Vector3 tvector = ray.Origin - v0;
            Vector3 dvector = tvector.Cross(ray.Direction);

            double u = v0v2.Dot(dvector) * invDet; 
            if (u < 0) return null;
            double v = -v0v1.Dot(dvector) * invDet; 
            if (v < 0 || (u+v) > 1.0) return null;
            double t = tvector.Dot(vnormal) * invDet; 
            if (t < 0) return null; */

            vposition = ray.Origin + t * ray.Direction;

            return new RayHit(vposition, vnormal, vincident, this.material);
        }

        /// <summary>
        /// The material of the triangle.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
