using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine
{
    public class App : GameWindow
    {
        private Stopwatch _stopwatch = new Stopwatch();

        public App(int width, int heigth) : base(width, heigth, GraphicsMode.Default)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            _stopwatch.Start();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Time.DeltaTime = (float) _stopwatch.Elapsed.TotalSeconds - Time.Elapsed;
            Time.Elapsed = (float)_stopwatch.Elapsed.TotalSeconds;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            var projectionMatrix = Matrix4.CreateOrthographic(800, 600, -1f, 1f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(Math.Sin(Time.Elapsed) * 200f - 75f, Math.Cos(Time.Elapsed) * 200f - 75f, 0f);

            GL.Color3(1f, 0.5f, 0.1f);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(0f, 0f);
            GL.Vertex2(150f, 150f);
            GL.Vertex2(0f, 150f);
            GL.End();

            SwapBuffers();
        }
    }
}
