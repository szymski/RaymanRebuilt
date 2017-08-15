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

        private List<Tuple<Graphics.Mesh, Graphics.Material>> GenerateAllMeshesAndMaterialsForNode(List<Tuple<Graphics.Mesh, Graphics.Material>> currentList, Node parentNode, bool flipTextureCoordinates = false, bool swapYZ = false)
        {
            foreach (Node child in parentNode.Children) {

                foreach (int mesh in child.MeshIndices) {
                    currentList.Add(GenerateMeshAndMaterial(child, Scene.Meshes[mesh], flipTextureCoordinates, swapYZ));
                }
                currentList.AddRange(GenerateAllMeshesAndMaterialsForNode(currentList, child, flipTextureCoordinates, swapYZ));
            }
            return currentList;
        }

        public List<Tuple<Graphics.Mesh, Graphics.Material>> GenerateAllMeshesAndMaterials(bool flipTextureCoordinates = false, bool swapYZ = false)
        {
            Viewport.Current.Logger.Log(new[] { "asset" }, "Generating " + Scene.MeshCount + " meshes and materials from asset");
            List<Tuple<Graphics.Mesh, Graphics.Material>> list = new List<Tuple<Graphics.Mesh, Graphics.Material>>();

            return GenerateAllMeshesAndMaterialsForNode(list, Scene.RootNode, flipTextureCoordinates, swapYZ);
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

        public Tuple<Graphics.Mesh, Graphics.Material> GenerateMeshAndMaterial(Assimp.Node assimpNode, Assimp.Mesh assimpMesh, bool flipTextureCoordinates = false, bool swapYZ = false)
        {
            var vertices = new List<Vertex>(assimpMesh.VertexCount);

            // Transform
            Matrix4x4 transform = assimpNode.Transform;
            Assimp.Node parent = assimpNode.Parent;
            while(parent != null)
            {
                transform *= parent.Transform;
                parent = parent.Parent;
            }

            for (int i = 0; i < assimpMesh.Vertices.Count; i++)
            {
                vertices.Add(new Vertex()
                {
                    Position = AssimpVectorToEngine(MultiplyAssimpVectorAndMatrix(assimpMesh.Vertices[i], transform), swapYZ),
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

        public List<Tuple<RenderableMesh, Graphics.Material>> GenerateAllRenderableMeshesAndMaterials(bool flipTextureCoordinates = false, bool swapYZ = false)
        {
            List<Tuple<RenderableMesh, Graphics.Material>> components = new List<Tuple<RenderableMesh, Graphics.Material>>();
            List<Tuple<Graphics.Mesh, Graphics.Material>> meshes = GenerateAllMeshesAndMaterials(flipTextureCoordinates, swapYZ);
            foreach (var mesh in meshes)
            {
                components.Add(new Tuple<RenderableMesh, Graphics.Material>(RenderableMesh.CreateManaged(mesh.Item1), mesh.Item2));
            }
            return components;
        }

        private Vector3D MultiplyAssimpVectorAndMatrix(Vector3D vector, Matrix4x4 mat)
        {
            Vector3D newVector = new Vector3D(0,0,0);
            
            newVector.X = mat.A1 * vector.X + mat.A2 * vector.Y + mat.A3 * vector.Z + mat.A4 * 1;
            newVector.Y = mat.B1 * vector.X + mat.B2 * vector.Y + mat.B3 * vector.Z + mat.B4 * 1;
            newVector.Z = mat.C1 * vector.X + mat.C2 * vector.Y + mat.C3 * vector.Z + mat.C4 * 1;

            return newVector;
            
        }

        private Vector3 AssimpVectorToEngine(Vector3D value, bool swapYZ=false)
        {
            if (swapYZ)
                return new Vector3(value.X, value.Z, value.Y);
            else
                return new Vector3(value.X, value.Y, value.Z);
        }
    }
}
