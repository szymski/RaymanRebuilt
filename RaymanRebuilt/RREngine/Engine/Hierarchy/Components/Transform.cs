using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Protobuf.Scene;

namespace RREngine.Engine.Hierarchy.Components
{
    public sealed class Transform : Component
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public Transform(GameObject owner) : base(owner)
        {
        }

        public Matrix4 ModelMatrix => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);

        new internal Protobuf.Scene.SceneSerialization.TransformProto Serialize()
        {
            Protobuf.Scene.SceneSerialization.TransformProto transformProto = new Protobuf.Scene.SceneSerialization.TransformProto();
            transformProto.Position = SceneSerializationUtil.Serialize(Position);
            transformProto.Quaternion = SceneSerializationUtil.Serialize(Rotation);
            transformProto.Scale = SceneSerializationUtil.Serialize(Scale);
            return transformProto;
        }
    }
}
