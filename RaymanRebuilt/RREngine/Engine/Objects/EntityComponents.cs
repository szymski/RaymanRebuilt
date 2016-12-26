using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Engine.Math;

namespace RREngine.Engine.Objects
{
    // This system doesn't let you interact with the components
    // at all via code, which is a problem, but I haven't thought
    // much of this through and it's not a pressing issue.


    public enum EntityComponent : uint
    {
        Geometry = 0
    }
    
    public partial class Entity
    {
        public void AddComponent(EntityComponent component)
        {
            if (component == EntityComponent.Geometry)
                components.Add(new Geometry());
        }



        public class Geometry
        {
            Mesh mesh = new Mesh();
        }
    }
}
