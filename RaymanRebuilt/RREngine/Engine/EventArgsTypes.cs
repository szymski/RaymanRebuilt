using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using RREngine.Engine.Input;

namespace RREngine.Engine
{
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

    public class WindowModeEventArgs : EventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Fullscreen { get; set; }
    }

    public class KeyRepeatEventArgs : EventArgs
    {
        public bool Repeat { get; set; }

        public KeyRepeatEventArgs(bool repeat)
        {
            Repeat = repeat;
        }
    }

    public class MouseLockedEventArgs : EventArgs
    {
        public bool Locked { get; set; }

        public MouseLockedEventArgs(bool locked)
        {
            Locked = locked;
        }
    }

    public class CursorVisibleEventArgs : EventArgs
    {
        public bool Visible { get; set; }

        public CursorVisibleEventArgs(bool visible)
        {
            Visible = visible;
        }
    }
}
