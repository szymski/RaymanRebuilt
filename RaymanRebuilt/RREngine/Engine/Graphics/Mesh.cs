using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine.Graphics
{
    /// <summary>
    /// Renderable OpenGL vertex buffer.
    /// </summary>
    public class Mesh
    {
        public int VertexArrayId { get; private set; }
        public int VertexBufferId { get; private set; }
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
            SendIndices();

            GL.BindVertexArray(0);
        }

        void SendVertices() {
            VertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);

            float[] vertexData = Vertices.Select(v => new float[] { v.Position.X, v.Position.Y, v.Position.Z }).Aggregate((a, b) => a.Concat(b).ToArray());
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * 4, vertexData, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
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
