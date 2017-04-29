using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics.Shaders;
using RREngine.Engine.Math;

namespace RREngine.Engine.Graphics
{
    public class BasicShapes
    {
        public BasicShapeShader Shader { get; }

        public RenderableMesh PlaneXY { get; }
        public RenderableMesh Line { get; }

        public BasicShapes()
        {
            Shader = new BasicShapeShader(Engine.AssetManager.LoadAsset<TextAsset>("shaders/basicshape.vs"),
                    Engine.AssetManager.LoadAsset<TextAsset>("shaders/basicshape.fs"));

            var plane = Plane.GenerateXY(Vector2.Zero, Vector2.One, Vector2.One);
            PlaneXY = RenderableMesh.CreateManaged(new Mesh(plane.Item1, plane.Item2));

            Line = RenderableMesh.CreateManaged(new Mesh(new []
            {
                new Vertex()
                {
                    Position = new Vector3(0, 0, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(1, 0, 0),
                }
            }, new [] { 0, 1 } ));
        }

        public void Use2D(Matrix4 projection)
        {
            var screen = Viewport.Current.Screen;

            Viewport.Current.ShaderManager.BindShader(Shader);
            Shader.ProjectionMatrix = projection;
            Shader.ViewMatrix = Matrix4.CreateTranslation(-screen.Width * 0.5f, -screen.Height * 0.5f, 0f) * Matrix4.CreateScale(1f, -1f, 1f);
        }

        public void Draw2DRectangle(Vector2 position, Vector2 size)
        {
            Shader.ModelMatrix = Matrix4.CreateScale(size.X, size.Y, 1f) * Matrix4.CreateTranslation(position.X, position.Y, 0f);
            PlaneXY.Draw();
        }

        public void Draw2DLine(Vector2 startPosition, Vector2 endPosition)
        {
            var diff = endPosition - startPosition;
            var angle = Mathf.Atan2(diff.Y, diff.X);

            Shader.ModelMatrix = Matrix4.CreateScale(diff.Length) * Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(startPosition.X, startPosition.Y, 0f);
            Line.Draw(PrimitiveType.Lines);
        }
    }
}
