using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine.Graphics
{
    public class RenderTarget
    {
        public Texture Texture { get; private set; }
        public int FboId { get; private set; }
        public int RbId { get; private set; }

        private int _previousFboId = 0;
        private int _previousRbId = 0;

        public RenderTarget(int width, int height)
        {
            Texture = new Texture(width, height);
            GenerateFrameBufferObject();
            GenerateRenderBuffer();
            RestoreBindings();
        }

        void GenerateFrameBufferObject()
        {
            _previousFboId = GL.GetInteger(GetPName.FramebufferBinding);

            FboId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FboId);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, Texture.Id, 0);
        }

        void GenerateRenderBuffer()
        {
            _previousRbId = GL.GetInteger(GetPName.RenderbufferBinding);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RbId);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth32fStencil8, Texture.Width, Texture.Height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FboId);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RbId);
        }

        void RestoreBindings()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _previousFboId);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _previousRbId);
        }

        public void Destroy()
        {
            GL.DeleteRenderbuffer(RbId);
            GL.DeleteFramebuffer(FboId);
            Texture.Destroy();
        }

        public void Resize(int width, int height)
        {
            if (Texture.Width == width && Texture.Height == height)
                return;

            // TODO: Deleting is probably not needed.
            GL.DeleteRenderbuffer(RbId);
            GL.DeleteFramebuffer(FboId);
           
            Texture.Resize(width, height);

            GenerateFrameBufferObject();
            GenerateRenderBuffer();
            RestoreBindings();
        }

        /// <summary>
        /// Binds the framebuffer.
        /// </summary>
        public void Bind()
        {
            _previousFboId = GL.GetInteger(GetPName.FramebufferBinding);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FboId);
        }

        /// <summary>
        /// Binds previous framebuffer.
        /// </summary>
        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _previousFboId);
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
