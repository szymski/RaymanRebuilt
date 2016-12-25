using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RREngine.Computations;

namespace RREngine.Objects
{
    /// <summary>
    /// A static mesh comprising vertex buffer objects.
    /// </summary>
    public class Brush
    {
        public Vec3 position;
        public Vec3 rotation;
        public Vec3 scale;

        // VBO stuff
    }



    /// <summary>
    /// A dynamic mesh capable of being manipulated in any fashion.
    /// </summary>
    public class Mesh
    {
        public Vec3 position;
        public Vec3 rotation;
        public Vec3 scale;

        public List<Face> faces;
        public List<Vertex> vertices;

        // Add vertex
        public void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex);
        }
        public void AddVertex(Vec3 position)
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

        // Add mesh
        public void AddMesh(Mesh mesh)
        {
            int countHold = vertices.Count;

            foreach (Vertex vert in mesh.vertices)
                AddVertex(vert.position.x + mesh.position.x, vert.position.y + mesh.position.y, vert.position.z + mesh.position.z);

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
        public Vec2 uv1, uv2, uv3, uv4;

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
        public Vec3 position;
        public GL.Color3 color = new GL.Color3(255, 255, 255);

        public Vertex(float x, float y, float z)
        {
            position.x = x;
            position.y = y;
            position.z = z;
        }
        public Vertex(Vec3 position)
        {
            this.position.x = position.x;
            this.position.y = position.y;
            this.position.z = position.z;
        }
    }
}
