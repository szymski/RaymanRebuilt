using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RREngine.Computations;

namespace RREngine.Objects
{
    public partial class Entity
    {
        public class Components
        {
            public class Transform
            {
                public Vec3 position;
                public Vec3 rotation;
                public Vec3 scale;
            }

            public class Light
            {

            }
        }
    }
    
}
