using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;
using RREngine.Engine.Math;

namespace RREngine.Engine.Hierarchy.Components
{
    public class CubeGenerator : Component
    {
        private MeshRenderer _meshRenderer;

        public Vector3 Size { get; set; } = new Vector3(1f,1f,1f);
        
        public Vector2 TexCoordScaling { get; set; } = Vector2.One;

        public CubeGenerator(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _meshRenderer = Owner.GetComponent<MeshRenderer>();
            Generate();
        }

        void Generate()
        {
            var CubeData = Cube.Generate(Size, TexCoordScaling);

            var mesh = RenderableMesh.CreateManaged(new Mesh(CubeData.Item1, CubeData.Item2));

            _meshRenderer.RenderableMesh = mesh;
        }
    }
}
