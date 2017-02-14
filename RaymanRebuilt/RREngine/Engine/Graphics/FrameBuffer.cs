using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    public class FrameBuffer
    {
        public int Id { get; private set; }

        public FrameBuffer()
        {
            Generate();
        }

        private void Generate()
        {
            Id = GL.GenFramebuffer();
        }

        public void Destroy()
        {
            GL.DeleteFramebuffer(Id);
        }

        public void ConnectTexture(Texture texture, FramebufferAttachment attachment)
        {
            Bind();

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment,
                texture.Target, texture.Id, 0);

            Unbind();
        }

        public void ConnectRenderBuffer(RenderBuffer renderBuffer)
        {
            Bind();
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderBuffer.Id);
            Unbind();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
