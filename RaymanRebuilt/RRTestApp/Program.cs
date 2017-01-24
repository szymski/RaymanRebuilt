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
using RREngine.Engine.Hierarchy;
using RREngine.Engine.Hierarchy.Components;
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
            Window window = new Window(1280, 720);

            var viewport = window.Viewport;

            Shader shader = new Shader(ShaderType.Fragment);
            Mesh mesh = null;

            Scene scene = new Scene();
            GameObject camera, teapot;

            window.Load += (sender, eventArgs) =>
            {


                shader.Compile(@"
#version 120

void main() {
    gl_FragColor = vec4(0.0, 1.0, 0.0, 1);
}
");
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
                GL.ClearColor(0.4f, 0.1f, 0.8f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

                scene.Render();
            };

            window.Run();
        }
    }
}
