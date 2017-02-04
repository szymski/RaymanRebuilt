using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Hierarchy.Components
{
    public abstract class Camera : Component
    {
        public Camera(GameObject owner) : base(owner)
        {
        }

        public abstract Matrix4 ProjectionMatrix { get; }

        /// <summary>
        /// Every object in the scene is multiplied by this matrix.
        /// </summary>
        public abstract Matrix4 ViewMatrix { get; }

        public abstract void Use();
    }
}
