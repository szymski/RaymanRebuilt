using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Gui
{
    public class GuiRenderer : IGuiRenderer
    {
        private BasicShapes _basicShapes;

        public GuiRenderer()
        {
            _basicShapes = Viewport.Current.BasicShapes;
        }

        public Vector4 Color
        {
            set { _basicShapes.Color = value; }
        }

        public void DrawLine(Vector2 startPosition, Vector2 endPosition)
        {
            _basicShapes.Draw2DLine(startPosition, endPosition);
        }

        public void DrawRectangle(Vector2 position, Vector2 size)
        {
            _basicShapes.Draw2DRectangle(position, size);
        }

        public void DrawRectangleOutline(Vector2 position, Vector2 size)
        {
            _basicShapes.Draw2DRectangleOutline(position, size);
        }
    }
}
