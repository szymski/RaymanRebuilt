using OpenTK;
using RREngine.Engine.Assets;

namespace RREngine.Engine.Graphics.Shaders
{
    public class SkyboxShader : Shader
    {
        public SkyboxShader(TextAsset vertex, TextAsset fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            AddUniform("u_cubemapTexture");
        }

        public Matrix4 ViewMatrix
        {
            set { SetUniform("u_viewMatrix", value); }
        }

        public Matrix4 ProjectionMatrix
        {
            set { SetUniform("u_projectionMatrix", value); }
        }

        public CubemapTexture CubemapTexture
        {
            set
            {
                value.Bind(0);
                SetUniform("u_cubemapTexture", 0);
            }
        }
    }
}
