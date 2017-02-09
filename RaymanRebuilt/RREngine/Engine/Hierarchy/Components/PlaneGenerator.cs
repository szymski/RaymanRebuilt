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

        public float TexCoordScaling { get; set; } = 1f;

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
            Vertex[] vertices = new []
            {
                new Vertex()
                {
                    Position = new Vector3(-MinBounds.X, 0, -MinBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(0, TexCoordScaling),
                },
                new Vertex()
                {
                    Position = new Vector3(MaxBounds.X, 0, -MinBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(TexCoordScaling, TexCoordScaling),
                },
                new Vertex()
                {
                    Position = new Vector3(MaxBounds.X, 0, MaxBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(TexCoordScaling, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-MinBounds.X, 0, MaxBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(0, 0),
                }
            };

            int[] faces = { 2, 1, 0, 3, 2, 0 };

            var mesh = new Mesh(vertices, faces);

            _meshRenderer.Mesh = mesh;
        }
    }
}
