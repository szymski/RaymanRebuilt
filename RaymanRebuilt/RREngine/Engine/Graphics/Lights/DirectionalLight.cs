using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Graphics.Lights
{
    public class DirectionalLight : BaseLight
    {
        public Vector3 Direction { get; set; } = new Vector3(-1f, -1f, -1f).Normalized();
    }
}
