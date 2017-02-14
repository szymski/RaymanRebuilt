#version 330

layout(location = 0) in vec3 mesh_vertexPos;
layout(location = 1) in vec3 mesh_vertexNormal;
layout(location = 2) in vec2 mesh_vertexTexCoord;

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

out vec2 vs_texCoord;

void main()
{
    mat4 mv = u_viewMatrix * u_modelMatrix;
    mat4 mvp = u_projectionMatrix *mv;
    
    vs_texCoord = mesh_vertexTexCoord;

    gl_Position = mvp * vec4(mesh_vertexPos, 1.0);
}