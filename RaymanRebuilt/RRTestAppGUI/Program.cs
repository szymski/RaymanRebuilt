using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine;
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics;
using RREngine.Engine.Graphics.Shaders;
using RREngine.Engine.Hierarchy;
using RREngine.Engine.Hierarchy.Components;
using RREngine.Engine.Input;
using RREngine.Engine.Math;

namespace RRTestAppGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Initialize();

            Window window = new Window(1280, 720);
            window.GameWindow.Title = "RaymanRebuilt WIP";

            var viewport = window.Viewport;

            RenderableMesh planeMesh = null;

            window.Load += (sender, eventArgs) =>
            {
                var plane = Plane.GenerateXY(Vector2.One, Vector2.One, Vector2.One);
                planeMesh = RenderableMesh.CreateManaged(new Mesh(plane.Item1, plane.Item2));
            };

            Vector2 resolutionBeforeChange = Vector2.Zero;

            viewport.UpdateFrame += (sender, eventArgs) =>
            {
                if (viewport.Keyboard.GetKeyDown(KeyboardKey.Escape))
                    window.GameWindow.Close();

                if (viewport.Keyboard.GetKeyUp(KeyboardKey.P))
                {
                    Engine.Logger.LogWarning("Doing something bad");
                    Engine.ResourceManager.FreeAllResources();
                }
            };

            viewport.RenderFrame += (sender, eventArgs) =>
            {
                GL.ClearColor(0.05f, 0.05f, 0.1f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

                var projMatrix = Matrix4.CreateOrthographic(viewport.Screen.Width, viewport.Screen.Height, -1f, 1f);

                var basicShapes = Viewport.Current.BasicShapes;

                basicShapes.Use2D(projMatrix);

                var screen = Viewport.Current.Screen;

                basicShapes.Shader.Color = new Vector4(1f, 0f, 0f, 1f);
                basicShapes.Draw2DRectangle(new Vector2(0f, 0f), new Vector2(100f, 100f));

                basicShapes.Shader.Color = new Vector4(1f, 1f, 0f, 1f);
                basicShapes.Draw2DLine(new Vector2(screen.Width * 0.5f, screen.Height * 0.5f), new Vector2(screen.Width * 0.5f, screen.Height * 0.5f) +
                    new Vector2(Mathf.Cos(viewport.Time.Elapsed), Mathf.Sin(viewport.Time.Elapsed)) * 100f);
            };

            window.Unload += (sender, eventArgs) =>
            {
                Engine.ResourceManager.FreeAllResources();
            };

            window.Run();
        }
    }
}
