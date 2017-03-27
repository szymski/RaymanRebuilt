using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Resources;

namespace RREngine.Engine.Graphics
{
    public class RenderBuffer : Resource
    {
        public int Id { get; private set; }

        private RenderBuffer(int width, int height)
        {
            Generate(width, height);
        }

        private void Generate(int width, int height)
        {
            Id = GL.GenRenderbuffer();

            Bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            Unbind();
        }

        public override void Destroy()
        {
            GL.DeleteRenderbuffer(Id);
        }

        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Id);
        }

        public void Unbind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public static RenderBuffer CreateManaged(int width, int height)
        {
            var resource = new RenderBuffer(width, height);
            Engine.ResourceManager.RegisterResource(resource);

            return resource;
        }

        public static RenderBuffer CreateUnmanaged(int width, int height)
        {
            return new RenderBuffer(width, height);
        }
    }
}
