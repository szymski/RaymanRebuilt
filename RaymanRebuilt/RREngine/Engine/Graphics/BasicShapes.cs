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
    public enum HorizontalTextAlignment
    {
        Left,
        Center,
        Right,
    }

    public enum VerticalTextAlignment
    {
        Top,
        Center,
        Bottom,
    }

    public class BasicShapes
    {
        public BasicShapeShader Shader { get; }

        public RenderableMesh PlaneXY { get; }
        public RenderableMesh Line { get; }
        private RenderableMesh _tempMesh;

        public Matrix4 Matrix { get; set; } = Matrix4.Identity;

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

            _tempMesh = RenderableMesh.CreateManaged();
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
                                 Matrix4.CreateTranslation(position.X, position.Y, 0f) * Matrix;
            PlaneXY.Draw();
        }

        public void Draw2DRectangleUV(Vector2 position, Vector2 size, Vector2 uvStart, Vector2 uvEnd)
        {
            var plane = Plane.GenerateXY(Vector2.Zero, Vector2.One, uvStart, uvEnd);

            _tempMesh.Update(new Mesh(plane.Item1, plane.Item2));

            Shader.ModelMatrix = Matrix4.CreateScale(size.X, size.Y, 1f) *
                                 Matrix4.CreateTranslation(position.X, position.Y, 0f) * Matrix;

            _tempMesh.Draw();
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
                                 Matrix4.CreateTranslation(startPosition.X, startPosition.Y, 0f) * Matrix;

            Line.Draw(PrimitiveType.Lines);
        }

        public void DrawText(Font font, Vector2 position, string text,
            HorizontalTextAlignment hAlignment = HorizontalTextAlignment.Left,
            VerticalTextAlignment vAlignment = VerticalTextAlignment.Top)
        {
            var textSize = MeasureText(font, text);
            float offsetX = 0, offsetY = 0;

            offsetX = hAlignment == HorizontalTextAlignment.Left ? 0
                : (hAlignment == HorizontalTextAlignment.Center ? -textSize.X / 2f : -textSize.X);
            offsetY = vAlignment == VerticalTextAlignment.Top ? 0
                : (vAlignment == VerticalTextAlignment.Center ? -textSize.Y / 2f : -textSize.Y);

            foreach (var c in text)
            {
                var glyph = font.Glyphs[c];

                Draw2DRectangleUV(Mathf.Round(position + new Vector2(offsetX + glyph.space[0], -glyph.offsetY + offsetY)), new Vector2(glyph.w, glyph.h), 
                    new Vector2(glyph.uStart, glyph.vStart),
                    new Vector2(glyph.uEnd, glyph.vEnd));

                position.X += glyph.w + glyph.space[2] + glyph.space[0];
            }
        }

        public Vector2 MeasureText(Font font, string text)
        {
            float x = 0;

            foreach (var c in text)
            {
                var glyph = font.Glyphs[c];
                x += glyph.w + glyph.space[2] + glyph.space[0];
            }

            return new Vector2(x, font.Height);
        }
    }
}
