using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine
{
    public class Time
    {
        /// <summary>
        /// Seconds elapsed since the application has started.
        /// </summary>
        public float Elapsed { get; set; }

        /// <summary>
        /// Seconds elapsed since the last frame.
        /// </summary>
        public float DeltaTime { get; set; }

        /// <summary>
        /// Delta time is multiplied by this value.
        /// </summary>
        public float TimeScale { get; set; } = 1f;
    }
}
