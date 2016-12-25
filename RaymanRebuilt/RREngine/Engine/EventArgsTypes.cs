using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine
{
    public class ResolutionEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public ResolutionEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
