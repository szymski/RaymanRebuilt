using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Unmanaged;
using OpenTK;
using RREngine.Engine.Graphics;
using Scene = Assimp.Scene;
using Vertex = RREngine.Engine.Graphics.Vertex;

namespace RREngine.Engine.Assets
{
    public class ModelAsset : Asset
    {
        public Scene Scene { get; private set; }

        public ModelAsset(Stream stream) : base(stream)
        {
            AssimpContext importer = new AssimpContext();
            Scene = importer.ImportFileFromStream(stream, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals);

            stream.Close();
        }

        public Graphics.Mesh GenerateMesh()
        {
            Viewport.Current.Logger.Log(new [] {"asset"}, "Generating mesh from asset");

            var assimpMesh = Scene.Meshes[0];

            var vertices = new List<Vertex>(assimpMesh.VertexCount);

            for (int i = 0; i < assimpMesh.Vertices.Count; i++)
                vertices.Add(new Vertex()
                {
                    Position = AssimpVectorToEngine(assimpMesh.Vertices[i]),
                    Normal = AssimpVectorToEngine(assimpMesh.Normals[i]),
                    TexCoord = assimpMesh.TextureCoordinateChannels[0].Count > 0 ? new Vector2(assimpMesh.TextureCoordinateChannels[0][i].X, assimpMesh.TextureCoordinateChannels[0][i].Y) : Vector2.Zero,
                }); 

            var indices = Scene.Meshes[0].GetIndices();

            Graphics.Mesh mesh = new Graphics.Mesh(vertices.ToArray(), indices);

            return mesh;
        }

        public RenderableMesh GenerateRenderableMesh()
        {
            return RenderableMesh.CreateManaged(GenerateMesh());
        }

        private Vector3 AssimpVectorToEngine(Vector3D value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
    }
}
