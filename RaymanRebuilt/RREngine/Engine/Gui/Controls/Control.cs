using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Gui.Controls
{
    public class Control
    {
        public Control Parent { get; set; }

        public Vector4 Margin { get; set; } = Vector4.Zero;
        public Vector4 Padding { get; set; } = Vector4.Zero;

        public Dock Dock { get; set; } = Dock.None;

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public List<Control> Children { get; } = new List<Control>();

    }
} 
