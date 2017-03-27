using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Graphics
{

    public class Mesh
    {
        public Vertex[] Vertices { get; private set; }
        public int[] Indices { get; private set; }

        public Mesh(Vertex[] vertices, int[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}
