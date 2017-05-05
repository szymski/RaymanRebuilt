using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Gui.Controls;

namespace RREngine.Engine.Gui
{
    public class GuiController
    {
        public IGuiRenderer Renderer { get; }

        public Panel Panel { get; set; }

        public GuiController(IGuiRenderer renderer)
        {
            Renderer = renderer;
            Panel = new Panel();
            Panel.Size = _size;
            Panel.Controller = this;
        }

        private Vector2 _size = new Vector2(256, 256);

        public Vector2 Size
        {
            get { return _size; }
            set
            {
                _size = value;
                Panel.Size = value;
            }
        }

        public void Think()
        {
            Panel.OnThinkInternal();
        }

        public void Render()
        {
            Panel.OnRenderInternal(Renderer);
        }

        public void OnMouseMoved(MouseMoveEventArgs args)
        {
            foreach (var control in Panel.GetChildrenRecursive())
                control.OnMouseMoved(args);
        }

        public void OnMouseButtonDown(MouseButtonEventArgs args)
        {
            foreach (var control in Panel.GetChildrenRecursive())
                control.OnMouseButtonDown(args);
        }

        public void OnMouseButtonUp(MouseButtonEventArgs args)
        {
            foreach (var control in Panel.GetChildrenRecursive())
                control.OnMouseButtonUp(args);
        }
    }
}
