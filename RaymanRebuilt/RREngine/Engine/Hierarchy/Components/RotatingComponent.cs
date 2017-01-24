using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Math;

namespace RREngine.Engine.Hierarchy.Components
{
    public class RotatingComponent : Component
    {
        private Transform _transform;
        private float yaw = 0f;

        public RotatingComponent(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        public override void OnUpdate()
        {
            yaw += Viewport.Current.Time.DeltaTime;
            _transform.Rotation = Quaternion.FromEulerAngles(0, yaw, 0);
        }
    }
}
