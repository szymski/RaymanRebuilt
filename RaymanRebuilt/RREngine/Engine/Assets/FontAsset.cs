using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Graphics;
using SharpFont;

namespace RREngine.Engine.Assets
{
    public class FontAsset : Asset
    {
        private Stream _stream;

        public FontAsset(Stream stream) : base(stream)
        {
            _stream = stream;
        }

        public Graphics.Font GetFont(float size)
        {
            var library = new Library();

            byte[] bytes = new byte[_stream.Length];
            _stream.Read(bytes, 0, bytes.Length);

            var face = new Face(library, bytes, 0);

            face.SetPixelSizes(0, (uint)size);

            return Graphics.Font.CreateManaged(face);
        }


    }
}
