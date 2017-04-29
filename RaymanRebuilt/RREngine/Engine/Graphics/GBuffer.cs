using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    /// <summary>
    /// Buffer for deferred rendering textures.
    /// </summary>
    public class GBuffer
    {
        public FrameBuffer FrameBuffer { get; private set; }

        public Texture2D TextureDiffuse { get; private set; }
        public Texture2D TexturePosition { get; private set; }
        public Texture2D TextureNormal { get; private set; }
        public Texture2D TextureTexCoord { get; private set; }
        public Texture2D TextureSpecular { get; private set; }
        public Texture2D TextureDepth { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public GBuffer(int width, int height)
        {
            Width = width;
            Height = height;

            GenerateTexturesAndBuffers();
        }

        private void GenerateTexturesAndBuffers()
        {
            FrameBuffer = FrameBuffer.CreateManaged();

            TextureDiffuse = Texture2D.CreateManaged(PixelInternalFormat.Rgba32f, PixelFormat.Rgba, PixelType.Float);
            TexturePosition = Texture2D.CreateManaged(PixelInternalFormat.Rgb32f, PixelFormat.Rgb, PixelType.Float);
            TextureNormal = Texture2D.CreateManaged(PixelInternalFormat.Rgb32f, PixelFormat.Rgb, PixelType.Float);
            TextureTexCoord = Texture2D.CreateManaged(PixelInternalFormat.Rg32f, PixelFormat.Rg, PixelType.Float);
            TextureSpecular = Texture2D.CreateManaged(PixelInternalFormat.Rg32f, PixelFormat.Rg, PixelType.Float);
            TextureDepth = Texture2D.CreateManaged(PixelInternalFormat.DepthComponent32f, PixelFormat.DepthComponent, PixelType.Float);

            Resize(Width, Height);

            FrameBuffer.ConnectTexture(TextureDiffuse, FramebufferAttachment.ColorAttachment0);
            FrameBuffer.ConnectTexture(TexturePosition, FramebufferAttachment.ColorAttachment1);
            FrameBuffer.ConnectTexture(TextureNormal, FramebufferAttachment.ColorAttachment2);
            FrameBuffer.ConnectTexture(TextureTexCoord, FramebufferAttachment.ColorAttachment3);
            FrameBuffer.ConnectTexture(TextureSpecular, FramebufferAttachment.ColorAttachment4);
            FrameBuffer.ConnectTexture(TextureDepth, FramebufferAttachment.DepthAttachment);

            FrameBuffer.Bind();
            GL.DrawBuffers(5, new[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
                DrawBuffersEnum.ColorAttachment3,
                DrawBuffersEnum.ColorAttachment4,
            });

            FrameBuffer.Bind();
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            TextureDiffuse.Resize(width, height);
            TexturePosition.Resize(width, height);
            TextureNormal.Resize(width, height);
            TextureTexCoord.Resize(width, height);
            TextureDepth.Resize(width, height);
            TextureSpecular.Resize(width, height);
        }

        public void Bind()
        {
            FrameBuffer.Bind();
        }

        public void Unbind()
        {
            FrameBuffer.Unbind();
        }
    }
}
