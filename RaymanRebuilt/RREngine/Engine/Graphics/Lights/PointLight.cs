using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Graphics.Lights
{
    public class PointLight : BaseLight
    {
        public Attenuation Attenuation { get; set; } = new Attenuation();
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
    }
}
