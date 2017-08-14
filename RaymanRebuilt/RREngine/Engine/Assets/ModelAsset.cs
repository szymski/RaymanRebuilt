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

        public List<Tuple<Graphics.Mesh, Graphics.Material>> GenerateAllMeshesAndMaterials(bool flipTextureCoordinates = false)
        {
            Viewport.Current.Logger.Log(new[] { "asset" }, "Generating " + Scene.MeshCount + " meshes and materials from asset");
            List<Tuple<Graphics.Mesh, Graphics.Material>> list = new List<Tuple<Graphics.Mesh, Graphics.Material>>();

            foreach (var assimpMesh in Scene.Meshes)
            {
                list.Add(GenerateMeshAndMaterial(assimpMesh, flipTextureCoordinates));
            }


            return list;
        }

        public List<Graphics.Mesh> GenerateAllMeshes()
        {
            Viewport.Current.Logger.Log(new[] { "asset" }, "Generating " + Scene.MeshCount + " meshes from asset");
            List<Graphics.Mesh> list = new List<Graphics.Mesh>();

            foreach (var assimpMesh in Scene.Meshes)
            {
                list.Add(GenerateMesh(assimpMesh));
            }


            return list;
        }

        public Graphics.Mesh GenerateMesh(Assimp.Mesh assimpMesh)
        {
            var vertices = new List<Vertex>(assimpMesh.VertexCount);

            for (int i = 0; i < assimpMesh.Vertices.Count; i++)
                vertices.Add(new Vertex()
                {
                    Position = AssimpVectorToEngine(assimpMesh.Vertices[i]),
                    Normal = AssimpVectorToEngine(assimpMesh.Normals[i]),
                    TexCoord = assimpMesh.TextureCoordinateChannels[0].Count > 0 ? new Vector2(assimpMesh.TextureCoordinateChannels[0][i].X, assimpMesh.TextureCoordinateChannels[0][i].Y) : Vector2.Zero,
                });

            var indices = assimpMesh.GetIndices();
            return new Graphics.Mesh(vertices.ToArray(), indices);
        }

        public Tuple<Graphics.Mesh, Graphics.Material> GenerateMeshAndMaterial(Assimp.Mesh assimpMesh, bool flipTextureCoordinates = false)
        {
            var vertices = new List<Vertex>(assimpMesh.VertexCount);

            for (int i = 0; i < assimpMesh.Vertices.Count; i++)
            {
                vertices.Add(new Vertex()
                {
                    Position = AssimpVectorToEngine(assimpMesh.Vertices[i]),
                    Normal = AssimpVectorToEngine(assimpMesh.Normals[i]),
                    TexCoord = (assimpMesh.TextureCoordinateChannels[0].Count > 0) ?
                        new Vector2(
                            assimpMesh.TextureCoordinateChannels[0][i].X,
                            flipTextureCoordinates ? (1.0f - assimpMesh.TextureCoordinateChannels[0][i].Y) : assimpMesh.TextureCoordinateChannels[0][i].Y
                        ) : Vector2.Zero
                });
            }

            var indices = assimpMesh.GetIndices();

            var assimpMaterial = Scene.Materials[assimpMesh.MaterialIndex];
            var assimpTexture = assimpMaterial.TextureDiffuse;
            var material = new Graphics.Material();
            if (assimpMaterial.TextureDiffuse.FilePath != null)
            {
                material.DiffuseTexture = Engine.AssetManager.LoadAsset<TextureAsset>(assimpMaterial.TextureDiffuse.FilePath).GenerateTexture();
            }
            material.SpecularPower = 0.8f;
            return new Tuple<Graphics.Mesh, Graphics.Material>(new Graphics.Mesh(vertices.ToArray(), indices), material);
        }

        public RenderableMesh GenerateFirstRenderableMesh()
        {
            return RenderableMesh.CreateManaged(GenerateMesh(Scene.Meshes[0]));
        }

        public List<RenderableMesh> GenerateAllRenderableMeshes()
        {
            List<RenderableMesh> components = new List<RenderableMesh>();
            List<Graphics.Mesh> meshes = GenerateAllMeshes();
            foreach (var mesh in meshes)
            {
                components.Add(RenderableMesh.CreateManaged(mesh));
            }
            return components;
        }

        public List<Tuple<RenderableMesh, Graphics.Material>> GenerateAllRenderableMeshesAndMaterials(bool flipTextureCoordinates = false)
        {
            List<Tuple<RenderableMesh, Graphics.Material>> components = new List<Tuple<RenderableMesh, Graphics.Material>>();
            List<Tuple<Graphics.Mesh, Graphics.Material>> meshes = GenerateAllMeshesAndMaterials(flipTextureCoordinates);
            foreach (var mesh in meshes)
            {
                components.Add(new Tuple<RenderableMesh, Graphics.Material>(RenderableMesh.CreateManaged(mesh.Item1), mesh.Item2));
            }
            return components;
        }

        private Vector3 AssimpVectorToEngine(Vector3D value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
    }
}
