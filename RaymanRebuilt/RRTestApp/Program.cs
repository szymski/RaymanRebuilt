using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine;
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics;
using RREngine.Engine.Hierarchy;
using RREngine.Engine.Hierarchy.Components;
using RREngine.Engine.Input;
using RREngine.Engine.Math;
using RREngine.Engine.Objects;
using Mesh = RREngine.Engine.Graphics.Mesh;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using ShaderType = RREngine.Engine.Graphics.ShaderType;
using Vertex = RREngine.Engine.Graphics.Vertex;

namespace RRTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(1280, 720);

            var viewport = window.Viewport;

            Shader shader = new Shader(ShaderType.Fragment | ShaderType.Vertex);
            viewport.ShaderManager.AddShader(shader);
            Mesh mesh = null;

            Scene scene = new Scene();
            GameObject camera, teapot;

            window.Load += (sender, eventArgs) =>
            {
                shader.Compile(File.ReadAllText("shaders/test.vs"), File.ReadAllText("shaders/test.fs"));
                shader.AddUniform("modelMatrix");
                shader.AddUniform("viewMatrix");
                shader.AddUniform("projectionMatrix");

                mesh = new ModelAsset("teapot.obj").GenerateMesh();

                camera = scene.CreateGameObject();
                camera.AddComponent<Transform>().Position = Vector3Directions.Backward * 20f;
                var camComponent = camera.AddComponent<PerspectiveCamera>();
                scene.CurrentCamera = camComponent;
                camera.AddComponent<FlyingCamera>();

                teapot = scene.CreateGameObject();
                var transform = teapot.AddComponent<Transform>();
                transform.Position = Vector3Directions.Forward * 10f;
                var renderer = teapot.AddComponent<MeshRenderer>();
                renderer.Mesh = mesh;
                teapot.AddComponent<RotatingComponent>();

                scene.Init();
            };

            viewport.Keyboard.KeyDown += (sender, eventArgs) => Console.WriteLine(eventArgs.Key);

            viewport.UpdateFrame += (sender, eventArgs) =>
            {
                if (viewport.Keyboard.GetKeyDown(KeyboardKey.Escape))
                    window.GameWindow.Close();

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.F))
                    Viewport.Current.Screen.IsFullscreen = !Viewport.Current.Screen.IsFullscreen;

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.L))
                {
                    Viewport.Current.Mouse.Locked = !Viewport.Current.Mouse.Locked;
                    Viewport.Current.Mouse.CursorVisible = !Viewport.Current.Mouse.Locked;
                }

                scene.Update();
            };

            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.05f, 0.05f, 0.2f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                viewport.ShaderManager.BindShader(shader);
                //shader.SetUniform("uniformFloat", Viewport.Current.Time.Elapsed);
                scene.Render();
                viewport.ShaderManager.UnbindShader();
            };

            window.Run();
        }
    }
}
