using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Graphics
{
    public class StandardShader : Shader
    {
        public StandardShader(string vertex, string fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
        {
            Compile(vertex, fragment);

            AddUniform("modelMatrix");
            AddUniform("viewMatrix");
            AddUniform("projectionMatrix");
        }

        public Matrix4 ModelMatrix
        {
            set { SetUniform("modelMatrix", value); }
        }

        public Matrix4 ViewMatrix
        {
            set { SetUniform("viewMatrix", value); }
        }

        public Matrix4 ProjectionMatrix
        {
            set { SetUniform("projectionMatrix", value); }
        }
    }
}
