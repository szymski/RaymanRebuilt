using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Gui.Controls
{
    public class Button : Control
    {
        public bool Down { get; set; }

        public Button(Control parent = null) : base(parent)
        {

        }

        public override void OnRender(IGuiRenderer renderer)
        {
            renderer.Color = new Vector4(1f, 1f, 1f, 1f);

            if (Hovered)
                renderer.Color = new Vector4(0.5f, 0.7f, 0.8f, 1f);

            if (Down)
                renderer.Color = new Vector4(0.2f, 0.4f, 0.6f, 1f);

            renderer.DrawRectangle(Vector2.Zero, Size);
        }

        public override void OnMouseButtonDown(MouseButtonEventArgs args)
        {
            base.OnMouseButtonDown(args);

            if (Hovered)
                Down = true;
        }

        public override void OnMouseButtonUp(MouseButtonEventArgs args)
        {
            base.OnMouseButtonUp(args);

            if (Hovered && Down)
                OnClick();

            Down = false;
        }

        public event EventHandler Click;

        public virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
