using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Input;
using RREngine.Engine.Math;

namespace RREngine.Engine.Hierarchy.Components
{
    public class FlyingCamera : Component
    {
        private Transform _transform;

        private float _pitch = 0f, _yaw = 0f;

        public FlyingCamera(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _transform = Owner.GetComponent<Transform>();
        }

        public override void OnUpdate()
        {
            _pitch -= Viewport.Current.Mouse.DeltaPosition.Y * Viewport.Current.Time.DeltaTime * 2f;
            _yaw -= Viewport.Current.Mouse.DeltaPosition.X * Viewport.Current.Time.DeltaTime * 2f;

            _transform.Rotation = Quaternion.FromEulerAngles(0, _yaw, 0);
            _transform.Rotation *= Quaternion.FromEulerAngles(0, 0, _pitch); // TODO: Pitch is swapped with roll, what am I doing wrong?

            if (Viewport.Current.Keyboard.GetKey(KeyboardKey.W))
                _transform.Position += _transform.Rotation * Vector3Directions.Forward * Viewport.Current.Time.DeltaTime * 10f;

            if (Viewport.Current.Keyboard.GetKey(KeyboardKey.S))
                _transform.Position += _transform.Rotation * Vector3Directions.Backward * Viewport.Current.Time.DeltaTime * 10f;

            if (Viewport.Current.Keyboard.GetKey(KeyboardKey.A))
                _transform.Position += _transform.Rotation * Vector3Directions.Left * Viewport.Current.Time.DeltaTime * 10f;

            if (Viewport.Current.Keyboard.GetKey(KeyboardKey.D))
                _transform.Position += _transform.Rotation * Vector3Directions.Right * Viewport.Current.Time.DeltaTime * 10f;

        }
    }
}
