using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RREngine.Engine.Math;

namespace RREngine.Engine.Objects
{
    /// <summary>
    /// A static mesh comprising vertex buffer objects.
    /// </summary>
    public class MeshStatic
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        // VBO stuff
    }



    /// <summary>
    /// A dynamic mesh capable of being manipulated in any fashion.
    /// </summary>
    public class Mesh
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public List<Face> faces;
        public List<Vertex> vertices;

        // Add vertex
        public void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex);
        }
        public void AddVertex(Vector3 position)
        {
            vertices.Add(new Vertex(position));
        }
        public void AddVertex(float x = 0, float y = 0, float z = 0)
        {
            vertices.Add(new Vertex(x, y, z));
        }

        // Add face
        public void AddFace(Face face)
        {
            faces.Add(face);
        }
        public void AddFace(int v1, int v2, int v3)
        {
            faces.Add(new Face(v1, v2, v3));
        }
        public void AddFace(int v1, int v2, int v3, int v4)
        {
            faces.Add(new Face(v1, v2, v3, v4));
        }

        // Combine mesh
        public void Combine(Mesh mesh)
        {
            int countHold = vertices.Count;

            foreach (Vertex vert in mesh.vertices)
                AddVertex(vert.position.X + mesh.position.X, vert.position.Y + mesh.position.Y, vert.position.Z + mesh.position.Z);

            foreach (Face face in mesh.faces)
            {
                Face face2 = new Face(face);
                face2.v1 += countHold;
                face2.v2 += countHold;
                face2.v3 += countHold;
                face2.v4 += countHold;
                AddFace(face2);
            }
        }
    }



    public class Face
    {
        public Mesh parentMesh;

        public bool isQuad;
        public int v1, v2, v3, v4;

        public bool hasUV;
        public Vector2 uv1, uv2, uv3, uv4;

        public bool hasTexture;
        public uint textureID;

        public Face(Face face)
        {
            parentMesh = face.parentMesh;
            isQuad = face.isQuad;
            v1 = face.v1; v2 = face.v2; v3 = face.v3; v4 = face.v4;
            hasUV = face.hasUV;
            uv1 = face.uv1; uv2 = face.uv2; uv3 = face.uv3; uv4 = face.uv4;
            hasTexture = face.hasTexture;
            textureID = face.textureID;

        }
        public Face(int v1, int v2, int v3)
        {
            isQuad = false;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
        public Face(int v1, int v2, int v3, int v4)
        {
            isQuad = true;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
        }
    }



    public class Vertex
    {
        public Mesh parentMesh;
        public Vector3 position;
        public Color color = Color.FromArgb(255, 255, 255);

        public Vertex(float x, float y, float z)
        {
            position.X = x;
            position.Y = y;
            position.Z = z;
        }
        public Vertex(Vector3 position)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;
            this.position.Z = position.Z;
        }
    }
}
