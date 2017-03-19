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
using OpenTK.Graphics.OpenGL4;
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
            if (Engine.Instance == null)
                throw new Exception("Engine isn't initialized.");

            SetAsCurrent();

            GameWindow = new GameWindow(width, height);
            Viewport = new Viewport();
            Viewport.Screen.OnWindowModeChanged(new WindowModeEventArgs()
            {
                Width = GameWindow.Width,
                Height = GameWindow.Height,
                Fullscreen = GameWindow.WindowState == WindowState.Fullscreen,
            });

            ConnectViewport(Viewport);

            //GameWindow.TargetRenderFrequency = 0f;
            //GameWindow.TargetRenderPeriod = 0f;
            //GameWindow.TargetUpdateFrequency = 0f;
            //GameWindow.TargetUpdatePeriod = 0f;

            //GameWindow.VSync = VSyncMode.Off;
        }

        void ConnectViewport(Viewport viewport)
        {
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

            OnLoad(EventArgs.Empty);
        }

        void OnGameWindowResize(object sender, EventArgs e)
        {
            SetAsCurrent();

            Viewport.Screen.OnWindowModeChanged(new WindowModeEventArgs()
            {
                Width = GameWindow.Width,
                Height = GameWindow.Height,
                Fullscreen = GameWindow.WindowState == WindowState.Fullscreen,
            });
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
                DeltaX = e.XDelta,
                DeltaY = e.YDelta,
            });

            if (_mouseCursorLocked)
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
            GameWindow.Size = new Size(e.Width, e.Height);

            Viewport.Screen.OnWindowModeChanged(e);
        }

        private void OnViewportKeyboardRepeatChangeRequested(object sender, KeyRepeatEventArgs e)
        {
            GameWindow.Keyboard.KeyRepeat = e.Repeat;
            Viewport.Keyboard.OnRepeatChanged(e);
        }

        private bool _mouseCursorLocked = false;

        private void OnViewportMouseLockedChangeRequest(object sender, MouseLockedEventArgs e)
        {
            _mouseCursorLocked = e.Locked;
            Viewport.Mouse.OnLockedChanged(e);
        }

        private void OnViewportMouseCursorVisibleRequested(object sender, CursorVisibleEventArgs e)
        {
            GameWindow.CursorVisible = e.Visible;
            Viewport.Mouse.OnCursorVisibleChanged(e);
        }

        #endregion

        [ThreadStatic]
        private static Window _current;

        public static Window Current => _current;
    }
}
