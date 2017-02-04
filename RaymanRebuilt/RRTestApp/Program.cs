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

            Mesh mesh = null;

            Scene scene = new Scene();
            SceneRenderer sceneRenderer = new SceneRenderer(scene);
            GameObject camera, teapot, plane;

            window.Load += (sender, eventArgs) =>
            {
                scene.SceneRenderer = sceneRenderer;

                mesh = AssetManager.Instance.LoadAsset<ModelAsset>("dragon.obj").GenerateMesh();
                var texture = AssetManager.Instance.LoadAsset<TextureAsset>("debug.png").GenerateTexture();

                camera = scene.CreateGameObject();
                camera.AddComponent<Transform>().Position = Vector3Directions.Backward * 20f;
                var camComponent = camera.AddComponent<PerspectiveCamera>();
                sceneRenderer.CurrentCamera = camComponent;
                camera.AddComponent<FlyingCamera>();

                plane = scene.CreateGameObject();
                plane.AddComponent<Transform>();
                plane.AddComponent<MeshRenderer>().Material = new Material()
                {
                    BaseColor = new Vector4(1f, 1f, 1f, 1f),
                    Texture = texture,
                };
                var planeGen = plane.AddComponent<PlaneGenerator>();
                planeGen.TexCoordScaling = 10f;
                planeGen.MinBounds = Vector2.One * 10;
                planeGen.MaxBounds = Vector2.One * 10;

                Random rand = new Random();

                for (int i = -2; i < -1; i++)
                {
                    Material mat = new Material()
                    {
                        BaseColor = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1),
                       // Texture = texture,
                    };

                    teapot = scene.CreateGameObject();
                    var transform = teapot.AddComponent<Transform>();
                    transform.Position = Vector3Directions.Forward * 10f + Vector3Directions.Right * 10f * i;
                    //transform.Scale *= 0.3f;
                    var renderer = teapot.AddComponent<MeshRenderer>();
                    renderer.Mesh = mesh;
                    renderer.Material = mat;
                    teapot.AddComponent<RotatingComponent>();
                }

                scene.Init();
                sceneRenderer.Init();
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

                sceneRenderer.Render();
            };

            window.Run();
        }
    }
}
