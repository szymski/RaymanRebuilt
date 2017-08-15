using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using OpenTK;
using RREngine.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Physics
{
    public static class PhysicsUtil
    {
        public static TriangleMeshShape CreateTriangleMeshShapeFromRenderableMesh(RenderableMesh mesh)
        {

            List<JVector> points = new List<JVector>();
            foreach(Vertex vertex in mesh.Vertices)
            {
                points.Add(new JVector(vertex.Position.X, vertex.Position.Y, vertex.Position.Z));
            }
            List<TriangleVertexIndices> indices = new List<TriangleVertexIndices>();
            for(int i=0;i<mesh.Indices.Length;i+=3)
            {
                indices.Add(new TriangleVertexIndices(mesh.Indices[i + 2], mesh.Indices[i + 1], mesh.Indices[i + 0])); // reverse tri order
            }

            Octree octree = new Octree(points, indices);
            octree.BuildOctree();
            TriangleMeshShape shape = new TriangleMeshShape(octree);
            return shape;
        }
    }
}
