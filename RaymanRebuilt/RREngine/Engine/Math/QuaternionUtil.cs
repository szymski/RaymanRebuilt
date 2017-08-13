using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Math
{
    public static class QuaternionUtil
    {
        public static Matrix3 QuaternionToMatrix(Quaternion quat)
        {

            float qx = quat.X;
            float qy = quat.Y;
            float qz = quat.Z;
            float qw = quat.W;

            Matrix3 matrix = new Matrix3(

                1 - 2 * qy*qy - 2 * qz*qz, 2 * qx * qy - 2 * qz * qw, 2 * qx * qz + 2 * qy * qw,
                2 * qx * qy + 2 * qz * qw, 1 - 2 * qx*qx - 2 * qz*qz, 2 * qy * qz - 2 * qx * qw,
                2 * qx * qz - 2 * qy * qw, 2 * qy * qz + 2 * qx * qw, 1 - 2 * qx*qx - 2 * qy*qy
            );

            return matrix;
        }
    }
}
