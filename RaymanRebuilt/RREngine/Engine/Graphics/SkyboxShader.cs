﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RREngine.Engine.Graphics
{
    public class SkyboxShader : Shader
    {
        public SkyboxShader(string vertex, string fragment) : base(ShaderType.Fragment | ShaderType.Vertex)
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