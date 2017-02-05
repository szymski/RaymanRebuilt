using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics
{
    public class BaseLight
    {
        public Vector3 Color { get; set; } = new Vector3(1f, 1f, 1f);
        public float Intensity { get; set; } = 1f;
    }

    public class DirectionalLight : BaseLight
    {
        public Vector3 Direction { get; set; } = new Vector3(-1f, -1f, -1f).Normalized();
    }

    public class StandardShader : Shader
    {
        public DirectionalLight DirectionalLight { get; } = new DirectionalLight();

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
            AddUniform("u_material.specularPower");
            AddUniform("u_material.specularIntensity");

            AddUniform("u_directionalLight.base.color");
            AddUniform("u_directionalLight.base.intensity");
            AddUniform("u_directionalLight.direction");
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
            SetUniform("u_directionalLight.base.color", DirectionalLight.Color);
            SetUniform("u_directionalLight.base.intensity", DirectionalLight.Intensity);
            SetUniform("u_directionalLight.direction", DirectionalLight.Direction);

            SetUniform("u_material.hasTexture", material.Texture != null);
            if (material.Texture != null)
            {
                GL.Enable(EnableCap.Texture2D);
                material.Texture.Bind();
                SetUniform("u_material.texture", 0);
            }

            SetUniform("u_material.baseColor", material.BaseColor);
            SetUniform("u_material.specularPower", material.SpecularPower);
            SetUniform("u_material.specularIntensity", material.SpecularIntensity);
        }
    }
}
