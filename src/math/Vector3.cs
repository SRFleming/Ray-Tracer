using System;

namespace RayTracer
{
    /// <summary>
    /// Immutable structure to represent a three-dimensional vector.
    /// </summary>
    public readonly struct Vector3
    {
        private readonly double x, y, z;

        /// <summary>
        /// Construct a three-dimensional vector.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Convert vector to a readable string.
        /// </summary>
        /// <returns>Vector as string in form (x, y, z)</returns>
        public override string ToString()
        {
            return "(" + this.x + "," + this.y + "," + this.z + ")";
        }

        /// <summary>
        /// Compute the length of the vector squared.
        /// This should be used if there is a way to perform a vector
        /// computation without needing the actual length, since
        /// a square root operation is expensive.
        /// </summary>
        /// <returns>Length of the vector squared</returns>
        public double LengthSq()
        {
            // Write your code here...
            double lengthSquared = this.x * this.x + this.y * this.y + this.z * this.z;
            return lengthSquared;
        }

        /// <summary>
        /// Compute the length of the vector.
        /// </summary>
        /// <returns>Length of the vector</returns>
        public double Length()
        {
            // Write your code here...
            double Vlength = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            return Vlength;
        }

        /// <summary>
        /// Compute a length 1 vector in the same direction.
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector3 Normalized()
        {
            // Write your code here...
            double magnitude = this.Length();
            return new Vector3(x/magnitude, y/magnitude, z/magnitude);
        }

        /// <summary>
        /// Compute the dot product with another vector.
        /// </summary>
        /// <param name="with">Vector to dot product with</param>
        /// <returns>Dot product result</returns>
        public double Dot(Vector3 with)
        {
            // Write your code here...
            double dotproduct = this.x * with.x + this.y * with.y + this.z * with.z;
            return dotproduct;
        }

        /// <summary>
        /// Compute the cross product with another vector.
        /// </summary>
        /// <param name="with">Vector to cross product with</param>
        /// <returns>Cross product result</returns>
        public Vector3 Cross(Vector3 with)
        {
            // Write your code here...
            double a = (this.y * with.z) - (this.z * with.y);
            double b = (this.z * with.x) - (this.x * with.z);
            double c = (this.x * with.y) - (this.y * with.x);
            return new Vector3(a, b, c);
        }

        /// <summary>
        /// Sum two vectors together (using + operator).
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Summed vector</returns>
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            // Write your code here...
            double q = a.x + b.x;
            double r = a.y + b.y;
            double s = a.z + b.z;
            return new Vector3(q, r, s);
        }

        /// <summary>
        /// Negate a vector (using - operator)
        /// </summary>
        /// <param name="a">Vector to negate</param>
        /// <returns>Negated vector</returns>
        public static Vector3 operator -(Vector3 a)
        {
            // Write your code here...
            return new Vector3(-a.x, -a.y, -a.z);
        }

        /// <summary>
        /// Subtract one vector from another.
        /// </summary>
        /// <param name="a">Original vector</param>
        /// <param name="b">Vector to subtract</param>
        /// <returns>Subtracted vector</returns>
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            // Write your code here...
            double q = a.x - b.x;
            double r = a.y - b.y;
            double s = a.z - b.z;
            return new Vector3(q, r, s);
        }

        /// <summary>
        /// Multiply a vector by a scalar value.
        /// </summary>
        /// <param name="a">Original vector</param>
        /// <param name="b">Scalar multiplier</param>
        /// <returns>Multiplied vector</returns>
        public static Vector3 operator *(Vector3 a, double b)
        {
            // Write your code here...
            double q = a.x * b;
            double r = a.y * b;
            double s = a.z * b;
            return new Vector3(q, r, s);
        }

        /// <summary>
        /// Multiply a vector by a scalar value (opposite operands).
        /// </summary>
        /// <param name="b">Scalar multiplier</param>
        /// <param name="a">Original vector</param>
        /// <returns>Multiplied vector</returns>
        public static Vector3 operator *(double b, Vector3 a)
        {
            // Write your code here...
            double q = a.x * b;
            double r = a.y * b;
            double s = a.z * b;
            return new Vector3(q, r, s);
        }

        /// <summary>
        /// Divide a vector by a scalar value.
        /// </summary>
        /// <param name="a">Original vector</param>
        /// <param name="b">Scalar divisor</param>
        /// <returns>Divided vector</returns>
        public static Vector3 operator /(Vector3 a, double b)
        {
            // Write your code here...
            double q = a.x / b;
            double r = a.y / b;
            double s = a.z / b;
            return new Vector3(q, r, s);
        }

        /// <summary>
        /// X component of the vector.
        /// </summary>
        public double X { get { return this.x; } }

        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public double Y { get { return this.y; } }

        /// <summary>
        /// Z component of the vector.
        /// </summary>
        public double Z { get { return this.z; } }
    }
}
