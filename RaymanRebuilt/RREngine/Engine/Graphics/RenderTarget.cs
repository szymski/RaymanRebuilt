using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    public class RenderTarget
    {
        public Texture2D Texture { get; private set; }
        public FrameBuffer FrameBuffer { get; private set; }
        public RenderBuffer RenderBuffer { get; private set; }

        public RenderTarget(int width, int height)
        {
            Texture = new Texture2D();
            Texture.Resize(width, height);
            GenerateFrameBufferObject();
            GenerateRenderBuffer();
            ConnectTexture();
        }

        private void GenerateFrameBufferObject()
        {
            FrameBuffer = new FrameBuffer();
        }

        private void GenerateRenderBuffer()
        {
            RenderBuffer = new RenderBuffer(Texture.Width, Texture.Height);
            FrameBuffer.ConnectRenderBuffer(RenderBuffer);
        }

        private void ConnectTexture()
        {
            FrameBuffer.ConnectTexture(Texture, FramebufferAttachment.ColorAttachment0);
        }

        public void Destroy()
        {
            RenderBuffer.Destroy();
            FrameBuffer.Destroy();
            Texture.Destroy();
        }

        public void Resize(int width, int height)
        {
            if (Texture.Width == width && Texture.Height == height)
                return;

            // TODO: Deleting is probably not needed.
            RenderBuffer.Destroy();
            FrameBuffer.Destroy();

            Texture.Resize(width, height);

            GenerateFrameBufferObject();
            GenerateRenderBuffer();
        }

        /// <summary>
        /// Binds the framebuffer.
        /// </summary>
        public void Bind()
        {
            FrameBuffer.Bind();
        }

        /// <summary>
        /// Binds default rendertarget.
        /// </summary>
        public void Unbind()
        {
            FrameBuffer.Unbind();
        }

        /// <summary>
        /// Same sa Texture.Bind().
        /// </summary>
        public void BindTexture()
        {
            Texture.Bind();
        }
    }
}
