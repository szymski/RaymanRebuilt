#version 330

layout(location = 0) in vec3 mesh_vertexPos;
layout(location = 1) in vec3 mesh_vertexNormal;
layout(location = 2) in vec2 mesh_vertexTexCoord;

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform vec3 u_cameraPosition;

out vec3 vs_vertexPos;
out vec3 vs_worldPos;
out vec3 vs_normal;
out vec2 vs_texCoord;

out mat4 vs_normalMatrix;

void main()
{
    mat4 mv = u_viewMatrix * u_modelMatrix;
    mat4 mvp = u_projectionMatrix * mv;
    vs_normalMatrix = transpose(inverse(u_modelMatrix));

    vs_vertexPos = (mv * vec4(mesh_vertexPos, 1.0)).xyz;
    vs_worldPos = (u_modelMatrix * vec4(mesh_vertexPos, 1.0)).xyz;
    vs_normal = normalize((vs_normalMatrix * vec4(mesh_vertexNormal, 1.0)).xyz);
    vs_texCoord = mesh_vertexTexCoord;

    gl_Position = mvp * vec4(mesh_vertexPos, 1.0);
}