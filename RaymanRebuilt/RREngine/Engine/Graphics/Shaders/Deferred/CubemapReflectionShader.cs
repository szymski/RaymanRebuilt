using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace RREngine.Engine.Graphics.Shaders.Deferred
{
    public class CubemapReflectionShader : Shader
    {
        public CubemapReflectionShader(string vertex, string fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            AddUniform("u_worldPos");
            AddUniform("u_diffuseColor");
            AddUniform("u_normal");
            AddUniform("u_specular");

            AddUniform("u_cubemap");

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

        public Vector3 CameraPosition
        {
            set { SetUniform("u_cameraPosition", value); }
        }

        public CubemapTexture Texture
        {
            set
            {
                GL.Enable(EnableCap.TextureCubeMap);
                value.Bind(10);
                SetUniform("u_cubemap", 10);
                GL.Disable(EnableCap.TextureCubeMap);
            }
        }
    }
}
