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
    public class PlaneGenerator : Component
    {
        private MeshRenderer _meshRenderer;

        public Vector2 MinBounds { get; set; } = new Vector2(0.5f, 0.5f);
        public Vector2 MaxBounds { get; set; } = new Vector2(0.5f, 0.5f);

        public Vector2 TexCoordScaling { get; set; } = Vector2.One;

        public PlaneGenerator(GameObject owner) : base(owner)
        {
        }

        public override void OnInit()
        {
            _meshRenderer = Owner.GetComponent<MeshRenderer>();
            Generate();
        }

        void Generate()
        {
            var plane = Plane.GenerateXZ(MinBounds, MaxBounds, TexCoordScaling);

            var mesh = new Mesh(plane.Item1, plane.Item2);

            _meshRenderer.Mesh = mesh;
        }
    }
}
