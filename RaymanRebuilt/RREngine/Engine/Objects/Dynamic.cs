using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine.Math;
using System.Dynamic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Objects
{
    public class Camera
    {
        public Vector3 position = new Vector3();
        public Vector3 rotation = new Vector3();

        public float fov = 60;
        public float clipNear;
        public float clipFar;
    }
    
    public partial class Entity
    {
        public string name = "";
        public Vector3 position;
        public Vector3 rotation;

        public List<object> components = new List<object>(); // derp
    }
}
