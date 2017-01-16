using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine
{
    public class Screen
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private bool _isFullscreen = false;
        public bool IsFullscreen
        {
            get { return _isFullscreen; }
            set
            {
                WindowModeChangeRequested?.Invoke(this, new WindowModeEventArgs()
                {
                    Fullscreen = value
                });
            }
        }

        public Screen()
        {
            Viewport.Current.ResolutionChanged += (sender, args) =>
            {
                Width = args.Width;
                Height = args.Height;
            };
        }

        public void SetResolution(int width, int height)
        {
            Viewport.Current.OnChangeResolution(new ResolutionEventArgs(width, height));
        }

        #region Events

        public event EventHandler<WindowModeEventArgs> WindowModeChangeRequested;
        public event EventHandler<WindowModeEventArgs> WindowModeChanged;

        public void OnWindowModeChanged(WindowModeEventArgs args)
        {
            WindowModeChanged?.Invoke(this, args);

            _isFullscreen = args.Fullscreen;
        }

        #endregion
    }
}
