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
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics;
using RREngine.Engine.Input;
using RREngine.Engine.Math;
using RREngine.Engine.Objects;
using Mesh = RREngine.Engine.Graphics.Mesh;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using ShaderType = RREngine.Engine.Graphics.ShaderType;
using Vertex = RREngine.Engine.Graphics.Vertex;

namespace RRTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(400, 400);

            var viewport = window.Viewport;

            Shader shader = new Shader(ShaderType.Fragment);
            Mesh mesh = null;

            window.Load += (sender, eventArgs) =>
            {


                shader.Compile(@"
#version 120

void main() {
    gl_FragColor = vec4(0.0, 1.0, 0.0, 1);
}
");
                mesh = new ModelAsset("teapot.obj").GenerateMesh();
            };

            viewport.Keyboard.KeyDown += (sender, eventArgs) => Console.WriteLine(eventArgs.Key);

            Vector3 cameraPos = new Vector3(0, 0, 10f);
            float cameraPitch = 0f, cameraYaw = 0f;
            Quaternion cameraRotation = Quaternion.Identity;

            viewport.UpdateFrame += (sender, eventArgs) =>
            {
                if (viewport.Keyboard.GetKeyDown(KeyboardKey.Escape))
                    window.GameWindow.Close();

                if (viewport.Keyboard.GetKey(KeyboardKey.W))
                    cameraPos += cameraRotation * Vector3Directions.Forward * viewport.Time.DeltaTime * 10f;

                if (viewport.Keyboard.GetKey(KeyboardKey.S))
                    cameraPos += cameraRotation * Vector3Directions.Backward * viewport.Time.DeltaTime * 10f;

                if (viewport.Keyboard.GetKey(KeyboardKey.A))
                    cameraPos += cameraRotation * Vector3Directions.Left * viewport.Time.DeltaTime * 10f;

                if (viewport.Keyboard.GetKey(KeyboardKey.D))
                    cameraPos += cameraRotation * Vector3Directions.Right * viewport.Time.DeltaTime * 10f;

                cameraPitch += -viewport.Mouse.DeltaPosition.Y * viewport.Time.DeltaTime * 2f;
                cameraYaw += -viewport.Mouse.DeltaPosition.X * viewport.Time.DeltaTime * 2f;

                cameraRotation = Quaternion.FromEulerAngles(0, cameraYaw, 0);
                cameraRotation *= Quaternion.FromEulerAngles(0, 0, cameraPitch); // TODO: WTF? Swapped pitch and roll?
            };

            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.4f, 0.1f, 0.8f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);
                var projectionMatrix2 = Matrix4.CreatePerspectiveFieldOfView(90f * Mathf.DegToRad, Viewport.Current.Screen.Width / (float)Viewport.Current.Screen.Height, 0.1f, 1000f);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projectionMatrix2);

                var modelMatrix2 = Matrix4.Identity;
                modelMatrix2 *= Matrix4.CreateRotationY(Viewport.Current.Time.Elapsed);
                modelMatrix2 *= Matrix4.CreateTranslation(-cameraPos);
                modelMatrix2 *= Matrix4.CreateFromQuaternion(cameraRotation.Inverted());
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelMatrix2);

                GL.Enable(EnableCap.Blend);
                shader.Bind();

                mesh.Draw();

                shader.Unbind();
            };

            window.Run();
        }
    }
}
