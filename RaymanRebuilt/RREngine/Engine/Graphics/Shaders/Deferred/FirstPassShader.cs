using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics.Shaders.Deferred
{
    public class FirstPassShader : Shader
    {
        public FirstPassShader(string vertex, string fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            //AddUniform("u_cameraPosition");

            AddUniform("u_material.baseColor");
            AddUniform("u_material.hasTexture");
            AddUniform("u_material.texture");

            AddUniform("u_material.specularPower");
            AddUniform("u_material.specularIntensity");
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

        public Vector3 CameraPosition
        {
            set { SetUniform("u_cameraPosition", value); }
        }

        public void UseMaterial(Material material)
        {
            SetUniform("u_material.hasTexture", material.Texture != null);
            if (material.Texture != null)
            {
                GL.Enable(EnableCap.Texture2D);
                material.Texture.Bind(0);
                SetUniform("u_material.texture", 0);
            }

            SetUniform("u_material.baseColor", material.BaseColor);
            SetUniform("u_material.specularPower", material.SpecularPower);
            SetUniform("u_material.specularIntensity", material.SpecularIntensity);
        }
    }
}
