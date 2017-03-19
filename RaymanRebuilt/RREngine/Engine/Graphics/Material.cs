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
        public Vector4 BaseColor { get; set; } = new Vector4(1f, 1f, 1f, 1f);

        public Texture2D DiffuseTexture { get; set; }
        public Texture2D NormalTexture { get; set; }

        public float SpecularPower { get; set; } = 15f;
        public float SpecularIntensity { get; set; } = 0.8f;

        public bool HasDiffuseTexture => DiffuseTexture != null;
        public bool HasNormalTexture => NormalTexture != null;
    }
}
