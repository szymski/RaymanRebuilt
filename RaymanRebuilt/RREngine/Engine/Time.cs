using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine
{
    public static class Time
    {
        /// <summary>
        /// Seconds elapsed since the application has started.
        /// </summary>
        public static float Elapsed { get; set; }

        /// <summary>
        /// Seconds elapsed since the last frame.
        /// </summary>
        public static float DeltaTime { get; set; }
    }
}
