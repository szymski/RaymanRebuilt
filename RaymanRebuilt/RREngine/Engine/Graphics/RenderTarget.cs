using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Resources;

namespace RREngine.Engine.Graphics
{
    public class RenderTarget : Resource
    {
        public Texture2D Texture { get; private set; }
        public FrameBuffer FrameBuffer { get; private set; }
        public RenderBuffer RenderBuffer { get; private set; }

        private RenderTarget(int width, int height)
        {
            Texture = Texture2D.CreateManaged();
            Texture.Resize(width, height);
            GenerateFrameBufferObject();
            GenerateRenderBuffer();
            ConnectTexture();
        }

        private void GenerateFrameBufferObject()
        {
            FrameBuffer = FrameBuffer.CreateManaged();
        }

        private void GenerateRenderBuffer()
        {
            RenderBuffer = RenderBuffer.CreateManaged(Texture.Width, Texture.Height);
            FrameBuffer.ConnectRenderBuffer(RenderBuffer);
        }

        private void ConnectTexture()
        {
            FrameBuffer.ConnectTexture(Texture, FramebufferAttachment.ColorAttachment0);
        }

        public override void Destroy()
        {
            Engine.ResourceManager.DecrementReferenceCount(FrameBuffer);
            Engine.ResourceManager.DecrementReferenceCount(RenderBuffer);
            Engine.ResourceManager.DecrementReferenceCount(Texture);
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
            ConnectTexture();
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

        public static RenderTarget CreateManaged(int width, int height)
        {
            var resource = new RenderTarget(width, height);
            Engine.ResourceManager.RegisterResource(resource);

            return resource;
        }

        public static RenderTarget CreateUnmanaged(int width, int height)
        {
            return new RenderTarget(width, height);
        }
    }
}
