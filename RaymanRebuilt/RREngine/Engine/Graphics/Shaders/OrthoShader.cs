using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Graphics.Shaders
{
    /// <summary>
    /// Simple shader for rendering 2D orthographics panels.
    /// </summary>
    public class OrthoShader : Shader
    {
        public OrthoShader(string vertexSource, string fragmentSource) : base(ShaderType.Vertex | ShaderType.Fragment)
        {
            Compile(vertexSource, fragmentSource);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            AddUniform("u_texture");
        }

        public Matrix4 ModelMatrix
        {
            set { SetUniform("u_modelMatrix", value); }
        }

        public Matrix4 ViewMatrix
        {
            set { SetUniform("u_viewMatrix", value); }
        }

        public Matrix4 ProjectionMatrix
        {
            set { SetUniform("u_projectionMatrix", value); }
        }

        public int Texture
        {
            set { SetUniform("u_texture", value); }
        }
    }
}
