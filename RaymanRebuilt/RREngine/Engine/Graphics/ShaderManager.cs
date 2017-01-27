using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine.Graphics
{
    public class ShaderManager
    {
        private List<Shader> _shaders = new List<Shader>();
        public IEnumerable<Shader> Shaders => _shaders.AsEnumerable();

        public Shader CurrentShader { get; private set; }

        public void AddShader(Shader shader)
        {
            _shaders.Add(shader);
        }

        public void DestroyShader(Shader shader)
        {
            shader.Destroy();
            _shaders.Remove(shader);
        }

        public void BindShader(Shader shader)
        {
            shader.Bind();
            CurrentShader = shader;
        }

        public void UnbindShader()
        {
            GL.UseProgram(0);
            CurrentShader = null;
        }
    }
}
