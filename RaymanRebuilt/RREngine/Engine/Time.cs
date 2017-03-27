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
        public float Elapsed { get; set; } = 0f;

        /// <summary>
        /// Seconds elapsed since the last frame multiplied by TimeScale.
        /// </summary>
        public float DeltaTime { get; set; } = 0f;

        /// <summary>
        /// Seconds elapsed since the last frame.
        /// </summary>
        public float RealDeltaTime { get; set; } = 0f;

        /// <summary>
        /// Delta time is multiplied by this value.
        /// </summary>
        public float TimeScale { get; set; } = 1f;

        /// <summary>
        /// Frames per second.
        /// </summary>
        public int FPS { get; set; } = 0;

        /// <summary>
        /// Average frames per second.
        /// </summary>
        public int AverageFPS { get; set; } = 0;
    }
}
