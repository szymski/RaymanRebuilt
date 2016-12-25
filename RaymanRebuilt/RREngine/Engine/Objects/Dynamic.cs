using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine.Math;

namespace RREngine.Engine.Objects
{
    public class Camera
    {
        public Vec3 position = new Vec3();
        public Vec3 rotation = new Vec3();

        public float fov = 60;
        public float clipNear;
        public float clipFar;
    }
    
    public partial class Entity
    {
        public string name = "";
        List<Components> components = new List<Components>();
    }
}
