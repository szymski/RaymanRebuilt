using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine
{
    public class Texture
    {
        public int Id { get; private set; }
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private int _previousTextureId = 0; // TODO: Is this necessary?

        public Texture()
        {
            GenerateTexture();
        }

        public void Destroy()
        {
            GL.DeleteTexture(Id);
        }

        public void GenerateTexture()
        {
            Id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, Id);

            // TODO: Move these to separate methods/properties
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        public void LoadImage(int width, int height, IntPtr data, PixelFormat format)
        {
            Width = width;
            Height = height;

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, format, PixelType.UnsignedByte, data);
        }

        public void Bind()
        {
            _previousTextureId = GL.GetInteger(GetPName.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, _previousTextureId);
        }
    }
}
