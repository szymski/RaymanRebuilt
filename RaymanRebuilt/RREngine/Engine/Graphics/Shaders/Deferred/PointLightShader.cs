using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RREngine.Engine.Assets;
using RREngine.Engine.Graphics.Lights;

namespace RREngine.Engine.Graphics.Shaders.Deferred
{
    public class PointLightShader : Shader
    {
        public PointLightShader(TextAsset vertex, TextAsset fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            AddUniform("u_worldPos");
            AddUniform("u_diffuseColor");
            AddUniform("u_normal");
            AddUniform("u_specular");

            AddUniform("u_pointLight.base.color");
            AddUniform("u_pointLight.base.intensity");
            AddUniform("u_pointLight.attenuation.constant");
            AddUniform("u_pointLight.attenuation.linear");
            AddUniform("u_pointLight.attenuation.exponential");
            AddUniform("u_pointLight.position");

            AddUniform("u_cameraPosition");
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

        public GBuffer GBuffer
        {
            set
            {
                GL.Enable(EnableCap.Texture2D);

                value.TexturePosition.Bind(0);
                SetUniform("u_worldPos", 0);

                value.TextureDiffuse.Bind(1);
                SetUniform("u_diffuseColor", 1);

                value.TextureNormal.Bind(2);
                SetUniform("u_normal", 2);

                value.TextureSpecular.Bind(3);
                SetUniform("u_specular", 3);
            }
        }

        public Vector3 Color
        {
            set { SetUniform("u_pointLight.base.color", value); }
        }

        public float Intensity
        {
            set { SetUniform("u_pointLight.base.intensity", value); }
        }

        public Vector3 Position
        {
            set { SetUniform("u_pointLight.position", value); }
        }
    
        public Vector3 CameraPosition
        {
            set { SetUniform("u_cameraPosition", value); }
        }

        public Attenuation Attenuation
        {
            set
            {
                SetUniform("u_pointLight.attenuation.constant", value.Constant);
                SetUniform("u_pointLight.attenuation.linear", value.Linear);
                SetUniform("u_pointLight.attenuation.exponential", value.Exponential);
            }
        }
    }
}
