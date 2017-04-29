using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RREngine.Engine.Assets;

namespace RREngine.Engine.Graphics.Shaders
{
    public class BasicShapeShader : Shader
    {
        public BasicShapeShader(TextAsset vertexSource, TextAsset fragmentSource) : base(ShaderType.Vertex | ShaderType.Fragment)
        {
            Compile(vertexSource, fragmentSource);

            AddUniform("u_modelMatrix");
            AddUniform("u_viewMatrix");
            AddUniform("u_projectionMatrix");

            //AddUniform("u_cameraPosition");

            AddUniform("u_material.baseColor");
            AddUniform("u_material.hasTexture");
            AddUniform("u_material.texture");

            //AddUniform("u_material.shaded");
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

        public Vector4 Color
        {
            set { SetUniform("u_material.baseColor", value); }   
        }

        public Texture2D Texture
        {
            set
            {
                if (value != null)
                {
                    value.Bind(0);
                    SetUniform("u_material.texture", 0);
                }

                SetUniform("u_material.hasTexture", value != null);
            }
        }
    }
}
