using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Graphics
{
    public class Material
    {
        public Texture Texture { get; set; }
        public Vector4 BaseColor { get; set; } = new Vector4(1, 1, 1, 1);

        public bool HasTexture => Texture != null;
    }
}
