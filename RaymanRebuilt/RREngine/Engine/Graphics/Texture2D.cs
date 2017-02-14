using System;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    public class Texture2D : Texture
    {
        public override TextureTarget Target => TextureTarget.Texture2D;

        /// <summary>
        /// Creates an uninitialized texture.
        /// Use LoadImage or Resize to initialize.
        /// </summary>
        public Texture2D(PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba, PixelFormat pixelFormat = PixelFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            PixelInternalFormat = pixelInternalFormat;
            PixelFormat = pixelFormat;
            PixelType = pixelType;

            GenerateTexture();
        }

        /// <summary>
        /// Creates a new texture with specified size.
        /// </summary>
        public Texture2D(int width, int height)
        {
            GenerateTexture();
            Resize(width, height);
        }

        public void LoadImage(int width, int height, IntPtr data, PixelFormat format)
        {
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat, width, height, 0, format, PixelType, data);
        }
    }
}
