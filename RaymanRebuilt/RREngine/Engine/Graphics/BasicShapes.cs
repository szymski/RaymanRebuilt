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

            var plane = Plane.GenerateXY(Vector2.Zero, Vector2.One, new Vector2(1f, -1f));
            PlaneXY = RenderableMesh.CreateManaged(new Mesh(plane.Item1, plane.Item2));

            Line = RenderableMesh.CreateManaged(new Mesh(new[]
            {
                new Vertex()
                {
                    Position = new Vector3(0, 0, 0),
                },
                new Vertex()
                {
                    Position = new Vector3(1, 0, 0),
                }
            }, new[] {0, 1}));
        }


        /// <summary>
        /// The texture to draw shapes with.
        /// Can be null - only color will be drawn then.
        /// </summary>
        public Texture2D Texture
        {
            set { Shader.Texture = value; }
        }

        /// <summary>
        /// The color to draw shapes with.
        /// </summary>
        public Vector4 Color
        {
            set { Shader.Color = value; }
        }
        
        /// <summary>
        /// Binds basic shape shader and prepares it.
        /// </summary>
        public void Use2D(Matrix4 projection)
        {
            var screen = Viewport.Current.Screen;

            Viewport.Current.ShaderManager.BindShader(Shader);
            Shader.ProjectionMatrix = projection;
            Shader.ViewMatrix = Matrix4.CreateTranslation(-screen.Width * 0.5f, -screen.Height * 0.5f, 0f) *
                                Matrix4.CreateScale(1f, -1f, 1f);
        }

        public void Draw2DRectangle(Vector2 position, Vector2 size)
        {
            Shader.ModelMatrix = Matrix4.CreateScale(size.X, size.Y, 1f) *
                                 Matrix4.CreateTranslation(position.X, position.Y, 0f);
            PlaneXY.Draw();
        }

        public void Draw2DRectangleOutline(Vector2 position, Vector2 size)
        {
            Draw2DLine(position, position + new Vector2(size.X, 0));
            Draw2DLine(position + new Vector2(size.X, 0), position + size);
            Draw2DLine(position + size, position + new Vector2(0, size.Y));
            Draw2DLine(position, position + new Vector2(0, size.Y));
        }

        public void Draw2DLine(Vector2 startPosition, Vector2 endPosition)
        {
            var diff = endPosition - startPosition;
            var angle = Mathf.Atan2(diff.Y, diff.X);

            Shader.ModelMatrix = Matrix4.CreateScale(diff.Length) * Matrix4.CreateRotationZ(angle) *
                                 Matrix4.CreateTranslation(startPosition.X, startPosition.Y, 0f);

            Line.Draw(PrimitiveType.Lines);
        }
    }
}
