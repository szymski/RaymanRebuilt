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
    }
}
