using System;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine.Graphics
{
    public class Texture
    {
        public int Id { get; private set; }
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private int _previousTextureId = 0; // TODO: Is this necessary?

        /// <summary>
        /// Creates an uninitialized texture.
        /// Use LoadImage or Resize to initialize.
        /// </summary>
        public Texture()
        {
            GenerateTexture();
        }

        /// <summary>
        /// Creates a new texture with specified size.
        /// </summary>
        public Texture(int width, int height)
        {
            GenerateTexture();
            Resize(width, height);
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

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public void LoadImage(int width, int height, IntPtr data, PixelFormat format)
        {
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, format, PixelType.UnsignedByte, data);
        }

        public void Resize(int width, int height)
        {
            if (Width == width && Height == height)
                return;

            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, (byte[])null);
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
