using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Resources;

namespace RREngine.Engine.Graphics
{
    /// <summary>
    /// Renderable OpenGL vertex buffer.
    /// </summary>
    public class RenderableMesh : Resource
    {
        public int VertexArrayId { get; private set; }

        public int VertexBufferId { get; private set; }
        public int NormalBufferId { get; private set; }
        public int TexCoordBufferId { get; private set; }
        public int IndexBufferId { get; private set; }

        private int _numIndices = 0;

        private RenderableMesh(Mesh mesh)
        {
            var vertices = mesh.Vertices;
            var indices = mesh.Indices;

            _numIndices = indices.Length;

            GenerateBuffer(vertices, indices);
        }

        public override void Destroy()
        {
            GL.DeleteVertexArray(VertexArrayId);
        }

        void GenerateBuffer(Vertex[] vertices, int[] indices)
        {
            VertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayId);

            Sendvertices(vertices, indices);
            SendNormals(vertices, indices);
            SendIndices(vertices, indices);
            SendTexCoords(vertices, indices);

            GL.BindVertexArray(0);
        }

        void Sendvertices(Vertex[] vertices, int[] indices) {
            VertexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId);

            float[] data = new float[vertices.Length * 3];

            for (int i = 0; i < vertices.Length; i++)
            {
                data[i * 3 + 0] = vertices[i].Position.X;
                data[i * 3 + 1] = vertices[i].Position.Y;
                data[i * 3 + 2] = vertices[i].Position.Z;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        void SendNormals(Vertex[] vertices, int[] indices)
        {
            NormalBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferId);

            float[] data = new float[vertices.Length * 3];

            for (int i = 0; i < vertices.Length; i++)
            {
                data[i * 3 + 0] = vertices[i].Normal.X;
                data[i * 3 + 1] = vertices[i].Normal.Y;
                data[i * 3 + 2] = vertices[i].Normal.Z;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        void SendTexCoords(Vertex[] vertices, int[] indices)
        {
            TexCoordBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, TexCoordBufferId);

            float[] data = new float[vertices.Length * 3];

            for (int i = 0; i < vertices.Length; i++)
            {
                data[i * 2 + 0] = vertices[i].TexCoord.X;
                data[i * 2 + 1] = vertices[i].TexCoord.Y;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
        }

        void SendIndices(Vertex[] vertices, int[] indices)
        {
            IndexBufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferId);

            int[] indexData = indices;
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexData.Length * 4, indexData, BufferUsageHint.StaticDraw);
        }

        public void Draw(PrimitiveType primitiveType = PrimitiveType.Triangles)
        {
            GL.BindVertexArray(VertexArrayId);
            GL.DrawElementsBaseVertex(primitiveType, _numIndices, DrawElementsType.UnsignedInt, IntPtr.Zero, 0);
            GL.BindVertexArray(0);
        }

        public static RenderableMesh CreateManaged(Mesh mesh)
        {
            var resource = new RenderableMesh(mesh);
            Engine.ResourceManager.RegisterResource(resource);

            return resource;
        }

        public static RenderableMesh CreateUnmanaged(Mesh mesh)
        {
            return new RenderableMesh(mesh);
        }
    }
}
