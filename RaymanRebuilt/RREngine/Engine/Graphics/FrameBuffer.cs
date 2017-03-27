using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Resources;

namespace RREngine.Engine.Graphics
{
    public class FrameBuffer : Resource
    {
        public int Id { get; private set; }

        private List<Resource> _resources = new List<Resource>();

        private FrameBuffer()
        {
            Generate();
        }

        private void Generate()
        {
            Id = GL.GenFramebuffer();
        }

        public override void Destroy()
        {
            GL.DeleteFramebuffer(Id);

            foreach (var resource in _resources)
                Engine.ResourceManager.DecrementReferenceCount(resource);
        }

        public void ConnectTexture(Texture texture, FramebufferAttachment attachment)
        {
            Bind();
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment,
                texture.Target, texture.Id, 0);
            Unbind();

            Engine.ResourceManager.IncrementReferenceCount(texture);
            _resources.Add(texture);
        }

        public void ConnectRenderBuffer(RenderBuffer renderBuffer)
        {
            Bind();
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.Id);
            Unbind();

            Engine.ResourceManager.IncrementReferenceCount(renderBuffer);
            _resources.Add(renderBuffer);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public static FrameBuffer CreateManaged()
        {
            var resource = new FrameBuffer();
            Engine.ResourceManager.RegisterResource(resource);

            return resource;
        }

        public static FrameBuffer CreateUnmanaged()
        {
            return new FrameBuffer();
        }
    }
}
