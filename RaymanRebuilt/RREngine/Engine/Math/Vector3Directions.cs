using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Math
{
    public static class Vector3Directions
    {
        public static Vector3 Forward => new Vector3(0, 0, -1);
        public static Vector3 Backward => new Vector3(0, 0, 1);

        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Left => new Vector3(-1, 0, 0);

        public static Vector3 Up => new Vector3(0, 1, 0);
        public static Vector3 Down => new Vector3(0, -1, 0);

    }
}
