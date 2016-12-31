using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Graphics
{
    public struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector2 TexCoord { get; set; }
        public Vector3 Normal { get; set; }
    }
}
