using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Gui.Controls
{
    public class Label : Control
    {
        public Label(Control parent = null) : base(parent)
        {

        }

        public override void OnRender(IGuiRenderer renderer)
        {
            //renderer.DrawText(0, 0, "Label");
        }
    }
}
