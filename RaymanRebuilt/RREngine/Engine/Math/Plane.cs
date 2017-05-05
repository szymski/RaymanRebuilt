using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Math
{
    public static class Plane
    {
        public static Tuple<Vertex[], int[]> GenerateXZ(Vector2 minBounds, Vector2 maxBounds, Vector2 texCoordScaling, float distance = 0)
        {
            Vertex[] vertices = new[]
            {
                new Vertex()
                {
                    Position = new Vector3(-minBounds.X, distance, -minBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(maxBounds.X, distance, -minBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(maxBounds.X, distance, maxBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-minBounds.X, distance, maxBounds.Y),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(0, 0),
                }
            };

            int[] faces = { 2, 1, 0, 3, 2, 0 };

            return new Tuple<Vertex[], int[]>(vertices, faces);
        }

        public static Tuple<Vertex[], int[]> GenerateXY(Vector2 minBounds, Vector2 maxBounds, Vector2 texCoordScaling, float distance = 0)
        {
            Vertex[] vertices = new[]
            {
                new Vertex()
                {
                    Position = new Vector3(-minBounds.X, -minBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(maxBounds.X, -minBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(maxBounds.X, maxBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-minBounds.X, maxBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(0, 0),
                }
            };

            int[] faces = { 2, 1, 0, 3, 2, 0 };

            return new Tuple<Vertex[], int[]>(vertices, faces);
        }

        public static Tuple<Vertex[], int[]> GenerateXY(Vector2 minBounds, Vector2 maxBounds, Vector2 uvStart, Vector2 uvEnd, float distance = 0)
        {
            Vertex[] vertices = new[]
            {
                new Vertex()
                {
                    Position = new Vector3(-minBounds.X, -minBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(uvStart.X, uvStart.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(maxBounds.X, -minBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(uvEnd.X, uvStart.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(maxBounds.X, maxBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(uvEnd.X, uvEnd.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(-minBounds.X, maxBounds.Y, distance),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(uvStart.X, uvEnd.Y),
                }
            };

            int[] faces = { 2, 1, 0, 3, 2, 0 };

            return new Tuple<Vertex[], int[]>(vertices, faces);
        }
    }
}
