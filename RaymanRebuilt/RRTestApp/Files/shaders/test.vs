#version 330

layout(location = 0) in vec3 mdl_vertexPos;
layout(location = 1) in vec3 mdl_vertexNormal;
layout(location = 2) in vec2 mdl_vertexTexCoord;

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform vec3 u_cameraPosition;

out vec3 vertexPos;
out vec3 worldVertexPos;
out vec3 vertexNormal;
out vec2 vertexTexCoord;

out mat4 normalMatrix;

void main()
{
    mat4 mvp = u_projectionMatrix * u_viewMatrix * u_modelMatrix;
    mat4 mv = u_viewMatrix * u_modelMatrix;
    normalMatrix = transpose(inverse(u_modelMatrix));

    vertexPos = (mv * vec4(mdl_vertexPos, 1.0)).xyz;
    worldVertexPos = (u_modelMatrix * vec4(mdl_vertexPos, 1.0)).xyz;
    vertexNormal = normalize((normalMatrix * vec4(mdl_vertexNormal, 1.0)).xyz);
    vertexTexCoord = mdl_vertexTexCoord;

    gl_Position = mvp * vec4(mdl_vertexPos, 1.0);
}