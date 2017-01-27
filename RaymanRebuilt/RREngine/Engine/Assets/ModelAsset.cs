using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Unmanaged;
using OpenTK;
using RREngine.Engine.Graphics;
using RREngine.Engine.Objects;
using Mesh = RREngine.Engine.Graphics.Mesh;
using Scene = Assimp.Scene;
using Vertex = RREngine.Engine.Graphics.Vertex;

namespace RREngine.Engine.Assets
{
    // TODO: Create Asset class and make this inherit from it
    public class ModelAsset
    {
        public Scene Scene { get; private set; }

        public ModelAsset(string filename)
        {
            AssimpContext importer = new AssimpContext();
            Scene = importer.ImportFile(filename, PostProcessSteps.Triangulate);
        }

        public Mesh GenerateMesh()
        {
            var assimpMesh = Scene.Meshes[0];

            var vertices = new List<Vertex>();

            for (int i = 0; i < assimpMesh.Vertices.Count; i++)
                vertices.Add(new Vertex()
                {
                    Position = AssimpVectorToEngine(assimpMesh.Vertices[i]),
                    Normal = AssimpVectorToEngine(assimpMesh.Normals[i]),
                }); 

            var indices = Scene.Meshes[0].GetIndices();

            Mesh mesh = new Mesh(vertices.ToArray(), indices);

            return mesh;
        }

        private Vector3 AssimpVectorToEngine(Vector3D value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
    }
}
