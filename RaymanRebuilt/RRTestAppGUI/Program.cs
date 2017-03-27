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

            window.Load += (sender, eventArgs) =>
            {
                
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



                var projMatrix = Matrix4.CreateOrthographic(viewport.Screen.Width, viewport.Screen.Height, -1f, 1f);
              

            };

            window.Unload += (sender, eventArgs) =>
            {
                Engine.ResourceManager.FreeAllResources();
            };

            window.Run();
        }
    }
}
