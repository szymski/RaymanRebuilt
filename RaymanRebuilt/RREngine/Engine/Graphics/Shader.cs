using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace RREngine.Engine.Graphics
{
    public enum ShaderType
    {
        Vertex = 1,
        Fragment = 2,
        // TODO: Add more? To think out.
    }

    public class Shader
    {
        public int ProgramId { get; private set; }
        public ShaderType Type { get; private set; }
        public bool Compiled { get; private set; } = false;

        List<int> _shaderIds = new List<int>();

        public Shader(ShaderType type)
        {
            Type = type;
        }

        public void Destroy()
        {
            DestroyShaders();
            GL.DeleteProgram(ProgramId);
        }

        void DestroyShaders()
        {
            foreach (var id in _shaderIds)
            {
                GL.DetachShader(ProgramId, id);
                GL.DeleteShader(id);
            }
        }

        public void Compile(string source)
        {
            if (Compiled)
                throw new Exception("This shader has already been compiled.");

            CreateProgram();

            int shaderId = CompileShader(source, Type);
            GL.AttachShader(ProgramId, shaderId);
            _shaderIds.Add(shaderId);

            LinkAndCheckForErrors();

            Compiled = true;
        }

        public void Compile(string vertexSource, string fragmentSource)
        {
            if (Compiled)
                throw new Exception("This shader has already been compiled.");

            if (Type == ShaderType.Fragment || Type == ShaderType.Vertex)
                throw new Exception("Cannot use mutliple shaders with one shader program.");

            CreateProgram();

            int vertexId = CompileShader(vertexSource, ShaderType.Vertex);
            GL.AttachShader(ProgramId, vertexId);
            _shaderIds.Add(vertexId);

            int fragmentId = CompileShader(fragmentSource, ShaderType.Fragment);
            GL.AttachShader(ProgramId, fragmentId);
            _shaderIds.Add(fragmentId);

            LinkAndCheckForErrors();

            Compiled = true;
        }

        void CreateProgram()
        {
            ProgramId = GL.CreateProgram();
        }

        /// <returns>ID of the compiled shader.</returns>
        int CompileShader(string source, ShaderType type)
        {
            var id = GL.CreateShader(type == ShaderType.Fragment ? OpenTK.Graphics.OpenGL.ShaderType.FragmentShader : OpenTK.Graphics.OpenGL.ShaderType.VertexShader);

            GL.ShaderSource(id, source);
            GL.CompileShader(id);

            try
            {
                CheckShaderCompilationError(id);
            }
            catch
            {
                GL.DeleteShader(id);
                throw;
            }

            return id;
        }

        /// <summary>
        /// Links the program and throws, if an error occured.
        /// Note: The shaders are automatically destroyed, when an error occurs.
        /// </summary>
        void LinkAndCheckForErrors()
        {
            try
            {
                GL.LinkProgram(ProgramId);
                CheckProgramLinkingError(ProgramId);

                GL.ValidateProgram(ProgramId);
                CheckProgramLinkingError(ProgramId);
            }
            catch
            {
                DestroyShaders();
                throw;
            }
        }

        void CheckShaderCompilationError(int shaderId)
        {
            int success;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out success);

            // When failed
            if (success == 0)
            {
                string info;
                GL.GetShaderInfoLog(shaderId, out info);

                throw new ShaderCompilationException(info);
            }
        }

        void CheckProgramLinkingError(int programId)
        {
            int success;
            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out success);

            // When failed
            if (success == 0)
            {
                string info;
                GL.GetProgramInfoLog(programId, out info);

                throw new ShaderLinkingException(info);
            }
        }

        public void Bind()
        {
            if(!Compiled)
                throw new Exception("This shader hasn't been compiled yet.");

            GL.UseProgram(ProgramId);
        }

        public void Unbind()
        {
            // TODO: Make this use the last program
            GL.UseProgram(0);
        }
    }

    public class ShaderCompilationException : Exception
    {
        public ShaderCompilationException(string message) : base(message)
        {
            
        }
    }

    public class ShaderLinkingException : Exception
    {
        public ShaderLinkingException(string message) : base(message)
        {

        }
    }
}
