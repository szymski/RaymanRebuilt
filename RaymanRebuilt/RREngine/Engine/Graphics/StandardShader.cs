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

    public class Attenuation
    {
        public float Constant { get; set; }
        public float Linear { get; set; } = 0.1f;
        public float Exponential { get; set; } = 1f;
    }

    public class PointLight : BaseLight
    {
        public Attenuation Attenuation { get; set; } = new Attenuation();
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
    }

    public class StandardShader : Shader
    {
        public const int MAX_POINT_LIGHTS = 8;

        public DirectionalLight DirectionalLight { get; } = new DirectionalLight();
        public List<PointLight> PointLights { get; } = new List<PointLight>();

        public StandardShader(string vertex, string fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            AddUniform("u_cameraPosition");

            AddUniform("u_ambientLight");

            AddUniform("u_material.hasTexture");
            AddUniform("u_material.texture");
            AddUniform("u_material.baseColor");
            AddUniform("u_material.specularPower");
            AddUniform("u_material.specularIntensity");

            AddUniform("u_directionalLight.base.color");
            AddUniform("u_directionalLight.base.intensity");
            AddUniform("u_directionalLight.direction");

            AddPointLightsUniforms();
        }

        private void AddPointLightsUniforms()
        {
            for (int i = 0; i < MAX_POINT_LIGHTS; i++)
            {
                AddUniform($"u_pointLights[{i}].base.color");
                AddUniform($"u_pointLights[{i}].base.intensity");
                AddUniform($"u_pointLights[{i}].attenuation.constant");
                AddUniform($"u_pointLights[{i}].attenuation.linear");
                AddUniform($"u_pointLights[{i}].attenuation.exponential");
                AddUniform($"u_pointLights[{i}].position");
            }
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
                material.Texture.Bind(0);
                SetUniform("u_material.texture", 0);
            }

            SetUniform("u_material.baseColor", material.BaseColor);
            SetUniform("u_material.specularPower", material.SpecularPower);
            SetUniform("u_material.specularIntensity", material.SpecularIntensity);

            for (int i = 0; i < MAX_POINT_LIGHTS; i++)
            {
                PointLight pointLight = i < PointLights.Count ? PointLights[i] : null;

                if (pointLight != null)
                {
                    SetUniform($"u_pointLights[{i}].base.color", pointLight.Color);
                    SetUniform($"u_pointLights[{i}].base.intensity", pointLight.Intensity);
                    SetUniform($"u_pointLights[{i}].attenuation.constant", pointLight.Attenuation.Constant);
                    SetUniform($"u_pointLights[{i}].attenuation.linear", pointLight.Attenuation.Linear);
                    SetUniform($"u_pointLights[{i}].attenuation.exponential", pointLight.Attenuation.Exponential);
                    SetUniform($"u_pointLights[{i}].position", pointLight.Position);
                }
                else
                    SetUniform($"u_pointLights[{i}].base.intensity", 0);
            }
        }
    }
}
