using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            SetAsCurrent();

            GameWindow = new GameWindow(width, height);
            Viewport = new Viewport();
            Viewport.OnResolutionChanged(new ResolutionEventArgs(width, height));

            GameWindow.Load += OnGameWindowLoad;

            GameWindow.Resize += OnGameWindowResize;

            GameWindow.UpdateFrame += OnGameWindowUpdateFrame;
            GameWindow.RenderFrame += OnGameWindowRenderFrame;

            GameWindow.Keyboard.KeyDown += OnGameWindowKeyDown;
            GameWindow.Keyboard.KeyUp += OnGameWindowKeyUp;

            GameWindow.Mouse.ButtonDown += OnGameWindowMouseButtonDown;
            GameWindow.Mouse.ButtonUp += OnGameWindowMouseButtonUp;
            GameWindow.Mouse.Move += OnGameWindowMouseMove;
            GameWindow.Mouse.WheelChanged += OnGameWindowMouseWheelChanged;

            Viewport.Screen.WindowModeChangeRequested += OnViewportScreenWindowModeChangeRequested;
            Viewport.Keyboard.RepeatChangeRequested += OnViewportKeyboardRepeatChangeRequested;
            Viewport.Mouse.ChangeLockedRequested += OnViewportMouseLockedChangeRequest;
            Viewport.Mouse.ChangeCursorVisibleRequested += OnViewportMouseCursorVisibleRequested;
        }

        public void Run()
        {
            SetAsCurrent();

            GameWindow.Run();
        }

        public void SetAsCurrent()
        {
            _current = this;
        }

        public event EventHandler<EventArgs> Load;

        #region GameWindow event handlers

        void OnLoad(EventArgs e)
        {
            SetAsCurrent();

            Load?.Invoke(this, e);
        }

        void OnGameWindowLoad(object sender, EventArgs e)
        {
            SetAsCurrent();

            ConnectViewport(Viewport);
            OnLoad(EventArgs.Empty);
        }

        void OnGameWindowResize(object sender, EventArgs e)
        {
            SetAsCurrent();

            Viewport.OnResolutionChanged(new ResolutionEventArgs(GameWindow.Width, GameWindow.Height));
        }

        void OnGameWindowUpdateFrame(object sender, EventArgs e)
        {
            SetAsCurrent();

            Viewport.OnUpdateFrame();
        }

        void OnGameWindowRenderFrame(object sender, EventArgs e)
        {
            SetAsCurrent();

            Viewport.OnRenderFrame();
            GameWindow.SwapBuffers();
        }

        private void OnGameWindowKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            SetAsCurrent();

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
            SetAsCurrent();

            Viewport.Keyboard.OnKeyUp(new KeyEventArgs()
            {
                Key = (KeyboardKey)e.Key,
                Control = e.Control,
                Alt = e.Alt,
                Shift = e.Shift,
                IsRepeat = e.IsRepeat,
            });
        }

        private void OnGameWindowMouseButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            SetAsCurrent();

            Viewport.Mouse.OnButtonDown(new MouseButtonEventArgs()
            {
                Button = (Input.MouseButton)e.Button,
                X = e.X,
                Y = e.Y,
            });
        }

        private void OnGameWindowMouseButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            SetAsCurrent();

            Viewport.Mouse.OnButtonUp(new MouseButtonEventArgs()
            {
                Button = (Input.MouseButton)e.Button,
                X = e.X,
                Y = e.Y,
            });
        }

        private void OnGameWindowMouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            SetAsCurrent();

            Viewport.Mouse.OnMove(new MouseMoveEventArgs()
            {
                X = e.X,
                Y = e.Y,
            });

            if (_mouseLocked)
                Cursor.Position = new Point(GameWindow.X + GameWindow.Width / 2, GameWindow.Y + GameWindow.Height / 2);
        }

        private void OnGameWindowMouseWheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            SetAsCurrent();

            Viewport.Mouse.OnWheelChanged(new MouseWheelEventArgs()
            {
                Delta = e.DeltaPrecise,
            });
        }

        private void OnViewportScreenWindowModeChangeRequested(object sender, WindowModeEventArgs e)
        {
            GameWindow.WindowState = e.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
            Viewport.Screen.OnWindowModeChanged(e);
            Viewport.OnResolutionChanged(new ResolutionEventArgs(GameWindow.Width, GameWindow.Height));
        }

        private void OnViewportKeyboardRepeatChangeRequested(object sender, KeyRepeatEventArgs e)
        {
            GameWindow.Keyboard.KeyRepeat = e.Repeat;
            Viewport.Keyboard.OnRepeatChanged(e);
        }

        private bool _mouseLocked = false;

        private void OnViewportMouseLockedChangeRequest(object sender, MouseLockedEventArgs e)
        {
            _mouseLocked = e.Locked;
            Viewport.Mouse.OnLockedChanged(e);
        }

        private void OnViewportMouseCursorVisibleRequested(object sender, CursorVisibleEventArgs e)
        {
            GameWindow.CursorVisible = e.Visible;
            Viewport.Mouse.OnCursorVisibleChanged(e);
        }

        #endregion

        void ConnectViewport(Viewport viewport)
        {
            Viewport.ChangeResolution += (sender, args) =>
            {
                GameWindow.ClientSize = new System.Drawing.Size(args.Width, args.Height);
            };
        }

        [ThreadStatic]
        private static Window _current;

        public static Window Current => _current;
    }
}
