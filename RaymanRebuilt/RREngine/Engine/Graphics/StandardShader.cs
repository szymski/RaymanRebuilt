using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    public class StandardShader : Shader
    {
        public StandardShader(string vertex, string fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            AddUniform("u_ambientLight");

            AddUniform("u_material.hasTexture");
            AddUniform("u_material.texture");
            AddUniform("u_material.baseColor");
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

        public Vector3 AmbientLight
        {
            set { SetUniform("u_ambientLight", value); }
        }

        public void UseMaterial(Material material)
        {
            SetUniform("u_material.hasTexture", material.Texture != null);
            if (material.Texture != null)
            {
                GL.Enable(EnableCap.Texture2D);
                material.Texture.Bind();
                SetUniform("u_material.texture", 0);
            }

            SetUniform("u_material.baseColor", material.BaseColor);
        }
    }
}
