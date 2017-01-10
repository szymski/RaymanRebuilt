using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using RREngine.Engine.Input;

namespace RREngine.Engine
{
    public class Window
    {
        public Viewport Viewport { get; private set; }
        public GameWindow GameWindow { get; private set; }

        public Window(int width = 800, int height = 600)
        {
            GameWindow = new GameWindow(width, height);
            Viewport = new Viewport();
            Viewport.OnResolutionChanged(new ResolutionEventArgs(width, height));

            GameWindow.Load += OnGameWindowLoad;

            GameWindow.Resize += OnGameWindowResize;

            GameWindow.UpdateFrame += OnGameWindowUpdateFrame;
            GameWindow.RenderFrame += OnGameWindowRenderFrame;

            GameWindow.Keyboard.KeyDown += OnGameWindowKeyDown;
            GameWindow.Keyboard.KeyUp += OnGameWindowKeyUp;
        }

        public void Run()
        {
            GameWindow.Run();
        }

        public event EventHandler<EventArgs> Load;

        #region GameWindow event handlers

        void OnLoad(EventArgs e)
        {
            Load?.Invoke(this, e);
        }

        void OnGameWindowLoad(object sender, EventArgs e)
        {
            ConnectViewport(Viewport);
            OnLoad(EventArgs.Empty);
        }

        void OnGameWindowResize(object sender, EventArgs e)
        {
            Viewport.OnResolutionChanged(new ResolutionEventArgs(GameWindow.Width, GameWindow.Height));
        }

        void OnGameWindowUpdateFrame(object sender, EventArgs e)
        {
            Viewport.OnUpdateFrame();
        }

        void OnGameWindowRenderFrame(object sender, EventArgs e)
        {
            Viewport.OnRenderFrame();
            GameWindow.SwapBuffers();
        }

        private void OnGameWindowKeyDown(object sender, KeyboardKeyEventArgs e)
        {
           Viewport.Keyboard.OnKeyDown(new KeyEventArgs()
           {
               Key = (KeyboardKey)e.Key,
               Control = e.Control,
               Alt = e.Alt,
               Shift = e.Shift,
               IsRepeat = e.IsRepeat,
           });
        }

        private void OnGameWindowKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            Viewport.Keyboard.OnKeyUp(new KeyEventArgs()
            {
                Key = (KeyboardKey)e.Key,
                Control = e.Control,
                Alt = e.Alt,
                Shift = e.Shift,
                IsRepeat = e.IsRepeat,
            });
        }

        #endregion

        void ConnectViewport(Viewport viewport)
        {
            Viewport.ChangeResolution += (sender, args) =>
            {
                GameWindow.ClientSize = new System.Drawing.Size(args.Width, args.Height);
            };
        }
    }
}
