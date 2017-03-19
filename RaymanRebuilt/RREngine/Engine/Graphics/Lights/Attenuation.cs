using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Graphics.Lights
{
    public class Attenuation
    {
        public float Constant { get; set; }
        public float Linear { get; set; } = 0.1f;
        public float Exponential { get; set; } = 1f;
    }
}
