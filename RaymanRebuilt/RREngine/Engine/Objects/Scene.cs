using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Objects
{
    public partial class Scene
    {
        public static List<Scene> scenes = new List<Scene>();
        

        public List<MeshStatic> brushes = new List<MeshStatic>();
        public List<Entity> entities = new List<Entity>();
        public uint[] textureIDs = new uint[2048]; // some number


        public MeshStatic AddBrush()
        {
            MeshStatic brush = new MeshStatic();
            brushes.Add(brush);
            return brush;
        }
        public Entity AddEntity()
        {
            Entity entity = new Entity();
            entities.Add(entity);
            return entity;
        }
    }
}
