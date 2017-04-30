using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Gui
{
    public interface IGuiRenderer
    {
        Vector4 Color { set; }

        void DrawLine(Vector2 startPosition, Vector2 endPosition);
        void DrawRectangle(Vector2 position, Vector2 size);
        void DrawRectangleOutline(Vector2 position, Vector2 size);
    }
}
