using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Math
{
    /// <summary>
    /// Represents a three-dimensional vector.
    /// </summary>
    public struct Vec3
    {
        public float x, y, z;
        public Vec3(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Gets the magnitudinal distance between two 3D coordinates.
        /// </summary>
        public static float GetDistance(Vec3 a, Vec3 b)
        {
            float b2 = Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
            return Mathf.Sqrt(b2 * b2 + (a.z - b.z) * (a.z - b.z));
        }

        #region operators
        public static bool operator ==(Vec3 a, Vec3 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z)
                return true;
            else
                return false;
        }
        public static bool operator !=(Vec3 a, Vec3 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z)
                return false;
            else
                return true;
        }
        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vec3 operator *(Vec3 a, float b)
        {
            return new Vec3(a.x * b, a.y * b, a.z * b);
        }
        public static Vec3 operator *(float a, Vec3 b)
        {
            return new Vec3(a * b.x, a * b.y, a * b.z);
        }
        public static Vec3 operator /(Vec3 a, float b)
        {
            return new Vec3(a.x / b, a.y / b, a.z / b);
        }
        public static Vec3 operator /(float a, Vec3 b)
        {
            return new Vec3(a / b.x, a / b.y, a / b.z);
        }
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        #endregion
    }


    /// <summary>
    /// Represents a two-dimensional vector.
    /// </summary>
    public struct Vec2
    {
        public float x, y;
        public Vec2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the magnitudinal distance between two 2D coordinates.
        /// </summary>
        public static float GetDistance(Vec2 a, Vec2 b)
        {
            return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
        }

        #region operators
        public static bool operator ==(Vec2 a, Vec2 b)
        {
            if (a.x == b.x && a.y == b.y)
                return true;
            else
                return false;
        }
        public static bool operator !=(Vec2 a, Vec2 b)
        {
            if (a.x == b.x && a.y == b.y)
                return false;
            else
                return true;
        }
        public static Vec2 operator *(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x * b.x, a.y * b.y);
        }
        public static Vec2 operator *(Vec2 a, float b)
        {
            return new Vec2(a.x * b, a.y * b);
        }
        public static Vec2 operator *(float a, Vec2 b)
        {
            return new Vec2(a * b.x, a * b.y);
        }
        public static Vec2 operator /(Vec2 a, float b)
        {
            return new Vec2(a.x / b, a.y / b);
        }
        public static Vec2 operator /(float a, Vec2 b)
        {
            return new Vec2(a / b.x, a / b.y);
        }
        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x + b.x, a.y + b.y);
        }
        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x - b.x, a.y - b.y);
        }
        #endregion
    }





    public struct Direction
    {
        public float cos;
        public float sin;

        public Direction(float cos, float sin)
        {
            this.cos = cos;
            this.sin = sin;
        }

        public static Direction AngleToDir(float angle)
        {
            return new Direction
            (
                Mathf.Cos(Mathf.PI * (angle / 180)),
                Mathf.Sin(Mathf.PI * (angle / 180))
            );
        }

        
        public static Vec3 Forward(Vec3 rotation)
        {
            return new Vec3
            (
                -AngleToDir(rotation.y).sin * AngleToDir(rotation.x).cos,
                AngleToDir(rotation.x).sin,
                AngleToDir(rotation.y).cos * AngleToDir(rotation.x).cos
            );
        }
        public static Vec3 Backward(Vec3 rotation)
        {
            return new Vec3
            (
                -AngleToDir(rotation.y + 180).sin * AngleToDir(rotation.x).cos,
                -AngleToDir(rotation.x).sin,
                AngleToDir(rotation.y).cos * AngleToDir(rotation.x + 180).cos
            );
        }
        public static Vec3 Left(Vec3 rotation) // Functional WIP --- Does not support anything with a z-axis rotation
        {
            rotation.y -= 90;
            return new Vec3
            (
                -AngleToDir(rotation.y).sin,
                0,
                AngleToDir(rotation.y).cos
            );
        }
        public static Vec3 Right(Vec3 rotation) // Functional WIP --- Does not support anything with a z-axis rotation
        {
            rotation.y += 90;
            return new Vec3
            (
                -AngleToDir(rotation.y).sin,
                0,
                AngleToDir(rotation.y).cos
            );
        }
    }
}
