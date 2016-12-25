using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine;
using RREngine.Engine.Objects;

namespace RREngine.Engine
{
    public class Scene
    {
        public static List<Scene> scenes = new List<Scene>();


        public List<Brush> meshes_static = new List<Brush>();
        public List<Mesh> meshes_dynamic = new List<Mesh>();
        public List<Entity> entities = new List<Entity>();
    }
}
