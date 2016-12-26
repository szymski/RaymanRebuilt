using System;
using System.Collections.Generic;
using RREngine.Engine.Objects;
using RREngine.Engine.Math;

namespace RREngine.Engine.Objects
{
    public static class Primitives
    {
        public static Mesh Mesh_Cube(float size_x, float size_y, float size_z, Vec3 position, Vec3 rotation)
        {
            Mesh cube = new Mesh();
            cube.position = position;
            cube.rotation = rotation;

            float x = size_x / 2;
            float y = size_y / 2;
            float z = size_z / 2;

            cube.AddVertex(-x, y, z);
            cube.AddVertex(x, y, z);
            cube.AddVertex(-x, -y, z);
            cube.AddVertex(x, -y, z);
            cube.AddVertex(-x, y, -z);
            cube.AddVertex(x, y, -z);
            cube.AddVertex(-x, -y, -z);
            cube.AddVertex(x, -y, -z);

            cube.AddFace(0, 1, 3, 2);
            cube.AddFace(4, 5, 7, 6);

            cube.AddFace(0, 1, 5, 4);
            cube.AddFace(2, 3, 7, 6);

            return cube;
        }

        public static Mesh Mesh_Plane(float size_x, float size_z, Vec3 position, Vec3 rotation)
        {
            Mesh cube = new Mesh();
            cube.position = position;
            cube.rotation = rotation;

            float x = size_x / 2;
            float z = size_z / 2;

            cube.AddVertex(-x, 0, -z);
            cube.AddVertex(-x, 0, z);
            cube.AddVertex(x, 0, -z);
            cube.AddVertex(x, 0, z);

            cube.AddFace(0, 1, 2);
            cube.AddFace(1, 2, 3);

            return cube;
        }

        public static Mesh Mesh_Cyllinder(float radius, float height, int sides, Vec3 position, Vec3 rotation)
        {
            Mesh cyl = new Mesh();
            cyl.position = position;
            cyl.rotation = rotation;

            float r = radius;
            float h = height;
            int s = sides;

            // bottom verts
            for (float i = 0; i < 2; i += 2f / s)
                cyl.AddVertex(Mathf.Cos(Mathf.PI * i) * r / 2, h / 2, Mathf.Sin(Mathf.PI * i) * r / 2);

            // top verts
            for (float i = 0; i < 2; i += 2f / s)
                cyl.AddVertex(Mathf.Cos(Mathf.PI * i) * r / 2, -h / 2, Mathf.Sin(Mathf.PI * i) * r / 2);
            
            // bottom faces
            for (int i = 0; i < s - 1; i++)
                cyl.AddFace(0, i + 1, i + 2);
            cyl.AddFace(s, 1, 0);

            // top faces
            for (int i = s; i < s * 2; i++)
                cyl.AddFace(s + 1, i + 1, i + 2);
            cyl.AddFace(s * 2 + 1, s + 1, s + 2);

            for (int i = 0; i < s - 1; i++)
                cyl.AddFace(i, i + 1, i + s + 1, i + s);

            return cyl;
        }

        public static Mesh Mesh_Cone(float radius, float height, int sides, Vec3 position, Vec3 rotation)
        {
            Mesh cone = new Mesh();
            cone.position = position;
            cone.rotation = rotation;

            float r = radius;
            float h = height;
            int s = sides;

            for (float i = 0; i < 2; i += 2f / s)
                cone.AddVertex(Mathf.Cos(Mathf.PI * i) * r / 2, 0, Mathf.Sin(Mathf.PI * i) * r / 2);
            cone.AddVertex(0, h, 0);

            // bottom faces
            for (int i = 0; i < s - 1; i++)
                cone.AddFace(0, i + 1, i + 2);
            cone.AddFace(s, 1, 0);

            // spike faces
            for (int i = 0; i < s - 1; i++)
                cone.AddFace(i, i + 1, s);

            return cone;
        }

        public static Mesh Mesh_Arrow(float radius, float length, Vec3 position, Vec3 rotation)
        {
            Mesh cyl = Primitives.Mesh_Cyllinder(radius, length, 8, new Vec3(0, length / 2, 0), new Vec3());
            Mesh cone = Primitives.Mesh_Cone(radius * 2.5f, radius * 4, 16, new Vec3(0, length, 0), new Vec3());

            Mesh arrow = new Mesh();
            arrow.position = position;
            arrow.rotation = rotation;

            arrow.AddMesh(cyl);
            arrow.AddMesh(cone);

            return arrow;
        }
    }
}
