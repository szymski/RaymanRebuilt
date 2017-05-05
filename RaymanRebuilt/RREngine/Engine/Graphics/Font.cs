using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Math;
using RREngine.Engine.Resources;
using SharpFont;

namespace RREngine.Engine.Graphics
{
    public struct Glyph
    {
        public int x, y;
        public int w, h;
        public float uStart, vStart;
        public float uEnd, vEnd;
        public Vector4 space;
        public int offsetY;
    }

    public class Font : Resource
    {
        private Face _face;

        public Texture2D Texture { get; private set; }

        public Dictionary<char, Glyph> Glyphs { get; } = new Dictionary<char, Glyph>();

        public int Height { get; private set; }

        private const int TextureSize = 512;

        private Font(Face face)
        {
            _face = face;

            Texture = Texture2D.CreateManaged();

            Height = face.Size.Metrics.Height.Ceiling();

            List<char> charList = new List<char>(256);
            for (int i = 32; i < 128; i++)
                charList.Add((char)i);

            byte[] bytes = new byte[TextureSize * TextureSize * 4];

            int x = 0, y = 0;

            int maxHeight = 0;
            for (int i = 0; i < charList.Count; i++)
            {
                char c = charList[i];

                var glyphIndex = face.GetCharIndex(c);
                face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
                face.Glyph.RenderGlyph(RenderMode.Normal);

                if (x + face.Glyph.Bitmap.Width >= TextureSize)
                {
                    x = 0;
                    y += maxHeight;
                    maxHeight = 0;
                }

                if (face.Glyph.Bitmap.Rows > maxHeight)
                    maxHeight += face.Glyph.Bitmap.Rows;

                for (int bY = 0; bY < face.Glyph.Bitmap.Rows; bY++)
                {
                    for (int bX = 0; bX < face.Glyph.Bitmap.Width; bX++)
                    {
                        var value = face.Glyph.Bitmap.BufferData[bY * face.Glyph.Bitmap.Width + bX];
                        var rawLocation = bY * TextureSize + bX + y * TextureSize + x;
                        bytes[rawLocation * 4] = value;
                        bytes[rawLocation * 4 + 1] = value;
                        bytes[rawLocation * 4 + 2] = value;
                        bytes[rawLocation * 4 + 3] = value;
                    }
                }

                Glyph glyphData = new Glyph()
                {
                    x = x,
                    y = y,
                    w = face.Glyph.Bitmap.Width,
                    h = face.Glyph.Bitmap.Rows,
                    uStart = x / (float)TextureSize,
                    vStart = y / (float)TextureSize,
                    uEnd = (x + face.Glyph.Bitmap.Width) / (float)TextureSize,
                    vEnd = (y + face.Glyph.Bitmap.Rows) / (float)TextureSize,
                };

                var additionalSpace = face.Glyph.Metrics.HorizontalAdvance.Ceiling() - glyphData.w;

                glyphData.space = new Vector4(additionalSpace / 2f, 0, additionalSpace / 2f, 0);
                glyphData.offsetY = face.Glyph.Metrics.HorizontalBearingY.Ceiling() - face.Size.Metrics.NominalHeight;

                Glyphs.Add(c, glyphData);

                x += face.Glyph.Bitmap.Width;
            }

            Texture.LoadImage(TextureSize, TextureSize, bytes, PixelFormat.Rgba);
        }

        public override void Destroy()
        {
            Engine.ResourceManager.DecrementReferenceCount(Texture);
        }

        public static Font CreateManaged(Face face)
        {
            var resource = CreateUnmanaged(face);
            Engine.ResourceManager.RegisterResource(resource);

            return resource;
        }

        public static Font CreateUnmanaged(Face face)
        {
            return new Font(face);
        }

    }
}
