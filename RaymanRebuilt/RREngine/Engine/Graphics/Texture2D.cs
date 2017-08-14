using System;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace RREngine.Engine.Graphics
{
    public class Texture2D : Texture
    {
        public override TextureTarget Target => TextureTarget.Texture2D;
        public bool HasTransparentPixels { get; set; } = false;

        /// <summary>
        /// Creates an uninitialized texture.
        /// Use LoadImage or Resize to initialize.
        /// </summary>
        private Texture2D(PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba, PixelFormat pixelFormat = PixelFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
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

        public void LoadImage(int width, int height, byte[] data, PixelFormat format)
        {
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat, width, height, 0, format, PixelType, data);
            UpdateTransparentPixelsFlag(data);
        }

        public void LoadImage(int width, int height, IntPtr data, PixelFormat format)
        {
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat, width, height, 0, format, PixelType, data);
            UpdateTransparentPixelsFlag(Width*Height*4, data);
        }

        public static Texture2D CreateManaged(PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba, PixelFormat pixelFormat = PixelFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            var resource = CreateUnmanaged(pixelInternalFormat, pixelFormat, pixelType);
            Engine.ResourceManager.RegisterResource(resource);

            return resource;
        }

        public static Texture2D CreateUnmanaged(PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba, PixelFormat pixelFormat = PixelFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
        {
            return new Texture2D(pixelInternalFormat, pixelFormat, pixelType);
        }

        private void UpdateTransparentPixelsFlag(byte[] data)
        {
            HasTransparentPixels = false;
            for(int i=3;i<data.Length;i+=4)
            {
                if (data[i]<255)
                {
                    HasTransparentPixels = true;
                    return;
                }
            }
        }

        private void UpdateTransparentPixelsFlag(int size, IntPtr data)
        {
            byte[] managedArray = new byte[size];
            Marshal.Copy(data, managedArray, 0, size);
            UpdateTransparentPixelsFlag(managedArray);
        }
    }
}
