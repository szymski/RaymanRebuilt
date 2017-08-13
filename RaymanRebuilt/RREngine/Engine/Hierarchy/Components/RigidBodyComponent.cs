using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using OpenTK;
using RREngine.Engine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Hierarchy.Components
{
    public class RigidBodyComponent : Component
    {
        private RigidBody RigidBody;
        public Shape Shape { get; set; }
        public Material Material { get; set; }
        private bool _static;
        public bool Static {
            get
            {
                return _static;
            }
            set
            {
                _static = value;
                if (RigidBody!=null)
                {
                    RigidBody.IsStatic = _static;
                }
            }
        }

        public RigidBodyComponent(GameObject owner) : base(owner)
        {

        }

        public override void OnInit()
        {
            RigidBody = new RigidBody(Shape, Material);
            RigidBody.IsStatic = Static;
            var transform = Owner.GetComponent<Transform>();
            var position = transform.Position;
            var rotation = transform.Rotation;
            var rotationMatrix = QuaternionUtil.QuaternionToMatrix(transform.Rotation);
            //RigidBody.Mass = 10;
            RigidBody.Position = new Jitter.LinearMath.JVector(position.X, position.Y, position.Z);
            RigidBody.Orientation = new Jitter.LinearMath.JMatrix(
                rotationMatrix.M11, rotationMatrix.M21, rotationMatrix.M31,
                rotationMatrix.M12, rotationMatrix.M22, rotationMatrix.M32,
                rotationMatrix.M13, rotationMatrix.M23, rotationMatrix.M33
            );
            Owner.Scene.PhysicsWorld.AddBody(RigidBody);
        }

        public override void OnUpdate()
        {
            var transform = Owner.GetComponent<Transform>();
            var position = transform.Position;

            position.X = RigidBody.Position.X;
            position.Y = RigidBody.Position.Y;
            position.Z = RigidBody.Position.Z;

            transform.Position = position;

            var bodyOrientation = RigidBody.Orientation;
            
            transform.Rotation = Quaternion.FromMatrix(new Matrix3(
                bodyOrientation.M11, bodyOrientation.M21, bodyOrientation.M31,
                bodyOrientation.M12, bodyOrientation.M22, bodyOrientation.M32,
                bodyOrientation.M13, bodyOrientation.M23, bodyOrientation.M33
            ));
        }

        public override void OnDestroy()
        {
            Owner.Scene.PhysicsWorld.RemoveBody(RigidBody);
        }
    }
}
