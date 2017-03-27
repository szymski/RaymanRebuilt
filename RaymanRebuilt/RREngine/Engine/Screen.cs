using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine
{
    public class Screen
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2 Size => new Vector2(Width, Height);

        private bool _isFullscreen = false;
        public bool IsFullscreen
        {
            get { return _isFullscreen; }
            set
            {
                WindowModeChangeRequested?.Invoke(this, new WindowModeEventArgs()
                {
                    Width = Width,
                    Height = Height,
                    Fullscreen = value
                });
            }
        }

        public Screen()
        {

        }

        public void SetResolution(int width, int height)
        {
            WindowModeChangeRequested?.Invoke(this, new WindowModeEventArgs()
            {
                Width = width,
                Height = height,
                Fullscreen = _isFullscreen,
            });
        }

        #region Events

        public event EventHandler<WindowModeEventArgs> WindowModeChangeRequested;
        public event EventHandler<WindowModeEventArgs> WindowModeChanged;

        public void OnWindowModeChanged(WindowModeEventArgs args)
        {
            WindowModeChanged?.Invoke(this, args);

            Width = args.Width;
            Height = args.Height;
            _isFullscreen = args.Fullscreen;

            Viewport.Current.Logger.Log(new[] { "screen" }, $"Changed screen mode {Width}x{Height}, fullscreen: {_isFullscreen}");
        }

        #endregion
    }
}
