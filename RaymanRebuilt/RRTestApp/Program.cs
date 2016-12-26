using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RREngine.Engine;
using RREngine.Engine.Math;
using RREngine.Engine.Objects;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace RRTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(400, 400);

            var viewport = window.Viewport;

            Texture texture = null;

            window.Load += (sender, eventArgs) =>
            {
                texture = new Texture();
                Bitmap bitmap = new Bitmap("doge.png");
                var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                texture.LoadImage(512, 512, data.Scan0, PixelFormat.Bgra);

                bitmap.UnlockBits(data);
            };

            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
                //var projectionMatrix = Matrix4.CreateOrthographic(Viewport.Current.Screen.Width, Viewport.Current.Screen.Height, -1f, 1f);
                var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(90f * Mathf.DegToRad, Viewport.Current.Screen.Width / (float)Viewport.Current.Screen.Height, 0.01f, 1000f);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projectionMatrix);

                var modelMatrix = Matrix4.Identity;
                modelMatrix *= Matrix4.CreateTranslation(-1f, -1f, 0f);
                modelMatrix *= Matrix4.CreateRotationY(Viewport.Current.Time.Elapsed);
                modelMatrix *= Matrix4.CreateTranslation(0f, 0f, -3f);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelMatrix);

                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                texture.Bind();

                GL.Color3(1f, 1f, 1f);

                GL.Begin(PrimitiveType.Quads);

                GL.TexCoord2(0f, 0f);
                GL.Vertex2(0f, 0f);

                GL.TexCoord2(0f, 1f);
                GL.Vertex2(0f, 2f);

                GL.TexCoord2(1f, 1f);
                GL.Vertex2(2f, 2f);

                GL.TexCoord2(1f, 0f);
                GL.Vertex2(2f, 0f);

                GL.End();
            };

            window.Run();

            MeshStatic b = Scene.scenes[0].AddBrush();
        }
    }
}
