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
            var vertices = Scene.Meshes[0].Vertices.Select(v => new Graphics.Vertex()
            {
                Position = new Vector3(v.X, v.Y, v.Z)
            }).ToArray();

            var indices = Scene.Meshes[0].GetIndices();

            Mesh mesh = new Mesh(vertices, indices);

            return mesh;
        }
    }
}
