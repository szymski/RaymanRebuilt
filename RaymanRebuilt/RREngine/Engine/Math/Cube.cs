using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Graphics;

namespace RREngine.Engine.Math
{
    public static class Cube
    {
        public static Tuple<Vertex[], int[]> Generate(Vector3 size, Vector2 texCoordScaling)
        {
            Vector3 _size = new Vector3(size.X * 0.5f, size.Y * 0.5f, size.Z * 0.5f);

            Vertex[] vertices = new[]
            {
                #region Top Plane (Y+)
                new Vertex()
                {
                    Position = new Vector3(-_size.X, _size.Y, -_size.Z),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, _size.Y, -_size.Z),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, _size.Y, _size.Z),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, _size.Y, _size.Z),
                    Normal = Vector3Directions.Up,
                    TexCoord = new Vector2(0, 0),
                },
                #endregion
                #region Bottom Plane (Y-)
                new Vertex()
                {
                    Position = new Vector3(-_size.X, -_size.Y, -_size.Z),
                    Normal = Vector3Directions.Down,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, -_size.Y, -_size.Z),
                    Normal = Vector3Directions.Down,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, -_size.Y, _size.Z),
                    Normal = Vector3Directions.Down,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, -_size.Y, _size.Z),
                    Normal = Vector3Directions.Down,
                    TexCoord = new Vector2(0, 0),
                },
                #endregion

                #region Left Plane (X-)
                new Vertex()
                {
                    Position = new Vector3(-_size.X, _size.Y, -_size.Z),
                    Normal = Vector3Directions.Left,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, _size.Y, _size.Z),
                    Normal = Vector3Directions.Left,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, -_size.Y, _size.Z),
                    Normal = Vector3Directions.Left,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, -_size.Y, -_size.Z),
                    Normal = Vector3Directions.Left,
                    TexCoord = new Vector2(0, 0),
                },
                #endregion
                #region Right Plane (X+)
                new Vertex()
                {
                    Position = new Vector3(_size.X, _size.Y, -_size.Z),
                    Normal = Vector3Directions.Right,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, _size.Y, _size.Z),
                    Normal = Vector3Directions.Right,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, -_size.Y, _size.Z),
                    Normal = Vector3Directions.Right,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, -_size.Y, -_size.Z),
                    Normal = Vector3Directions.Right,
                    TexCoord = new Vector2(0, 0),
                },
                #endregion

                #region Front Plane (Z-)
                new Vertex()
                {
                    Position = new Vector3(-_size.X, _size.Y, -_size.Z),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, _size.Y, -_size.Z),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, -_size.Y, -_size.Z),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, -_size.Y, -_size.Z),
                    Normal = Vector3Directions.Backward,
                    TexCoord = new Vector2(0, 0),
                },
                #endregion
                #region Rear Plane (Z+)
                new Vertex()
                {
                    Position = new Vector3(-_size.X, _size.Y, _size.Z),
                    Normal = Vector3Directions.Forward,
                    TexCoord = new Vector2(0, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, _size.Y, _size.Z),
                    Normal = Vector3Directions.Forward,
                    TexCoord = new Vector2(texCoordScaling.X, texCoordScaling.Y),
                },
                new Vertex()
                {
                    Position = new Vector3(_size.X, -_size.Y, _size.Z),
                    Normal = Vector3Directions.Forward,
                    TexCoord = new Vector2(texCoordScaling.X, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(-_size.X, -_size.Y, _size.Z),
                    Normal = Vector3Directions.Forward,
                    TexCoord = new Vector2(0, 0),
                }
                #endregion


            };

            int[] faces = { 2,1,0, 3,2,0,
                            4,5,6, 4,6,7,
                            10,9,8,11,10,8,
                            12,13,14,12,14,15,
                            16,17,18,16,18,19,
                            22,21,20,23,22,20
                           };

            return new Tuple<Vertex[], int[]>(vertices, faces);
        }

    }
}
