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
using RREngine.Engine.Gui;
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

            GuiController guiController = null;

            window.Load += (sender, eventArgs) =>
            {
               guiController = new GuiController(new GuiRenderer());
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

                guiController.Think();
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

                guiController.Render();
            };

            window.Unload += (sender, eventArgs) =>
            {
                Engine.ResourceManager.FreeAllResources();
            };

            window.Run();
        }
    }
}
