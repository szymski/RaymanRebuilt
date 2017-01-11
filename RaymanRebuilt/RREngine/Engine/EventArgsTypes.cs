using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using RREngine.Engine.Input;

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

    public class KeyEventArgs : EventArgs
    {
        public KeyboardKey Key { get; set; }
        public bool Control { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }
        public bool IsRepeat { get; set; }
    }

    public class MouseButtonEventArgs : EventArgs
    {
        public Input.MouseButton Button { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MouseMoveEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MouseWheelEventArgs : EventArgs
    {
        public float Delta { get; set; }
    }
}
