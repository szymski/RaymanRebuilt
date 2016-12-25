using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RREngine.Engine;

namespace RRTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(400, 400);

            var viewport = window.Viewport;
            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
                var projectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height, -1f, 1f);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projectionMatrix);

                var modelMatrix = Matrix4.Identity;
                modelMatrix *= Matrix4.CreateTranslation(-75f, -75f, 0);
                modelMatrix *= Matrix4.CreateRotationZ((float)Math.Sin(Viewport.Current.Time.Elapsed) * (float)Math.PI);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelMatrix);

                GL.Begin(PrimitiveType.Triangles);
                GL.Color3(1f, 0.5f, 0.1f);
                GL.Vertex2(0f, 0f);
                GL.Color3(0.1f, 1f, 0.5f);
                GL.Vertex2(150f, 150f);
                GL.Color3(1f, 0.1f, 0.5f);
                GL.Vertex2(0f, 150f);
                GL.End();
            };

            window.Run();
        }
    }
}
