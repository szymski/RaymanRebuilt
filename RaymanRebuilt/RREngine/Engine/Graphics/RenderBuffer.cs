using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    public class RenderBuffer
    {
        public int Id { get; private set; }

        public RenderBuffer(int width, int height)
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

        public void Destroy()
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
    }
}
