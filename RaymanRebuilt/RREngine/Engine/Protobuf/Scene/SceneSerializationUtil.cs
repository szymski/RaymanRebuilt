using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Protobuf.Scene
{
    public static class SceneSerializationUtil
    {
        public static SceneSerialization.Vector3Proto Serialize(OpenTK.Vector3 vector3)
        {
            var vector3proto = new SceneSerialization.Vector3Proto();
            vector3proto.X = vector3.X;
            vector3proto.Y = vector3.Y;
            vector3proto.Z = vector3.Z;
            return vector3proto;
        }

        public static SceneSerialization.QuaternionProto Serialize(OpenTK.Quaternion quaternion)
        {
            var quaternionProto = new SceneSerialization.QuaternionProto();
            quaternionProto.X = quaternion.X;
            quaternionProto.Y = quaternion.Y;
            quaternionProto.Z = quaternion.Z;
            quaternionProto.W = quaternion.W;
            return quaternionProto;
        }
    }
}
