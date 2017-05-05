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
using RREngine.Engine.Gui.Controls;
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

            Font font = null;

            window.Load += (sender, eventArgs) =>
            {
                var fontAsset = Engine.AssetManager.LoadAsset<FontAsset>("arial.ttf");
                font = fontAsset.GetFont(18f);

                guiController = new GuiController(new GuiRenderer()
                {
                    DrawDebug = true,
                });

                Viewport.Current.Mouse.Move += (o, a) =>
                {
                    guiController.OnMouseMoved(a);
                };

                Viewport.Current.Mouse.ButtonDown += (o, a) =>
                {
                    guiController.OnMouseButtonDown(a);
                };

                Viewport.Current.Mouse.ButtonUp += (o, a) =>
                {
                    guiController.OnMouseButtonUp(a);
                };

                var panel1 = guiController.Panel;
                panel1.Render += (o, renderer) =>
                {
                    renderer.Color = new Vector4(0.5f, 0.2f, 0.8f, 1f);
                    renderer.DrawRectangle(Vector2.Zero, panel1.Size);
                };

                var panel4 = new Panel();
                panel4.Dock = Dock.Left;
                panel4.Margin = new Vector4(4, 10, 20, 40);
                panel1.Children.Add(panel4);
                panel4.Render += (o, renderer) =>
                {
                    renderer.Color = new Vector4(0.2f, 0.5f, 0.8f, 1f);
                    renderer.DrawRectangle(Vector2.Zero, panel4.Size);
                };
                panel4.Think += (o, args1) =>
                {
                    panel4.Width = 64 + Mathf.Sin(viewport.Time.Elapsed) * 40f;
                };

                var panel2 = new Panel();
                panel2.Dock = Dock.Bottom;
                panel2.Height = 32;
                panel1.Children.Add(panel2);
                panel2.Render += (o, renderer) =>
                {
                    renderer.Color = new Vector4(0.8f, 0.5f, 0.2f, 1f);
                    renderer.DrawRectangle(Vector2.Zero, panel2.Size);
                };

                var label = new Label(panel2);
                

                var panel5 = new Panel();
                panel5.Dock = Dock.Right;
                panel5.Margin = new Vector4(6, 6, 6, 6);
                panel1.Children.Add(panel5);
                panel5.Render += (o, renderer) =>
                {
                    renderer.Color = new Vector4(0.2f, 0.8f, 0.5f, 1f);
                    renderer.DrawRectangle(Vector2.Zero, panel5.Size);
                };

                var panel3 = new Panel();
                panel3.Margin = new Vector4(2, 2, 4, 8);
                panel3.Dock = Dock.Bottom;
                panel1.Children.Add(panel3);
                panel3.Render += (o, renderer) =>
                {
                    renderer.Color = new Vector4(0.5f, 0.8f, 0.2f, 1f);
                    renderer.DrawRectangle(Vector2.Zero, panel3.Size);
                };

                var panel6 = new Button();
                panel6.Dock = Dock.Fill;
                panel1.Children.Add(panel6);
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
                GL.ClearColor(0.05f, 0.25f, 0.5f, 1f);
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                GL.Viewport(0, 0, Viewport.Current.Screen.Width, Viewport.Current.Screen.Height);

                var projMatrix = Matrix4.CreateOrthographic(viewport.Screen.Width, viewport.Screen.Height, -1f, 1f);

                var basicShapes = Viewport.Current.BasicShapes;

                basicShapes.Use2D(projMatrix);

                var screen = Viewport.Current.Screen;

                viewport.BasicShapes.Texture = null;
                guiController.Render();

                viewport.BasicShapes.Use2D(projMatrix);
                viewport.BasicShapes.Texture = font.Texture;
                viewport.BasicShapes.Color = Vector4.One;
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
                //viewport.BasicShapes.Draw2DRectangle(viewport.Screen.Size / 2f, Vector2.One * font.Texture.Width);


                viewport.BasicShapes.DrawText(font, new Vector2(400 + Mathf.Sin(viewport.Time.Elapsed) * 5f, 400),
                    "This is a test",
                    HorizontalTextAlignment.Center, VerticalTextAlignment.Center);

                viewport.BasicShapes.Texture = null;
                viewport.BasicShapes.Draw2DRectangleOutline(new Vector2(400 + Mathf.Sin(viewport.Time.Elapsed) * 5f, 400), Vector2.One * font.Height);
            };

            window.Unload += (sender, eventArgs) =>
            {
                Engine.ResourceManager.FreeAllResources();
            };

            window.Run();
        }
    }
}
