using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    /// <summary>
    /// Renderable OpenGL vertex buffer.
    /// </summary>
    public class Mesh
    {
        public int VertexArrayId { get; private set; }

        public int VertexBufferId { get; private set; }
        public int NormalBufferId { get; private set; }
        public int TexCoordBufferId { get; private set; }
        public int IndexBufferId { get; private set; }

        public Vertex[] Vertices { get; private set; }
        public int[] Indices { get; private set; }

        public Mesh(Vertex[] vertices, int[] indices)
        {
            Vertices = vertices;
            Indices = indices;

            GenerateBuffer();
        }

        public void Destroy()
        {
            GL.DeleteVertexArray(VertexArrayId);
        }

        void GenerateBuffer()
        {
            VertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayId);

            SendVertices();
            SendNormals();
            SendIndices();
            SendTexCoords();

            GL.BindVertexArray(0);
        }

        void SendVertices() {
            VertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);

            float[] data = new float[Vertices.Length * 3];

            for (int i = 0; i < Vertices.Length; i++)
            {
                data[i * 3 + 0] = Vertices[i].Position.X;
                data[i * 3 + 1] = Vertices[i].Position.Y;
                data[i * 3 + 2] = Vertices[i].Position.Z;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        void SendNormals()
        {
            NormalBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferId);

            float[] data = new float[Vertices.Length * 3];

            for (int i = 0; i < Vertices.Length; i++)
            {
                data[i * 3 + 0] = Vertices[i].Normal.X;
                data[i * 3 + 1] = Vertices[i].Normal.Y;
                data[i * 3 + 2] = Vertices[i].Normal.Z;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        void SendTexCoords()
        {
            TexCoordBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, TexCoordBufferId);

            float[] data = new float[Vertices.Length * 3];

            for (int i = 0; i < Vertices.Length; i++)
            {
                data[i * 2 + 0] = Vertices[i].TexCoord.X;
                data[i * 2 + 1] = Vertices[i].TexCoord.Y;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
        }

        void SendIndices()
        {
            IndexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferId);

            int[] indexData = Indices;
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexData.Length * 4, indexData, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            GL.BindVertexArray(VertexArrayId);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, Indices.Length);
            GL.DrawElementsBaseVertex(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero, 0);
            GL.BindVertexArray(0);
        }
    }
}
