#version 330

layout(location = 0) in vec3 mdl_vertexPos;
layout(location = 1) in vec3 mdl_vertexNormal;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

out vec3 vertexPos;
out vec3 vertexNormal;

out mat4 normalMatrix;

void main()
{
    mat4 mvp = projectionMatrix * viewMatrix * modelMatrix;
    mat4 mv = viewMatrix * modelMatrix;
    normalMatrix = transpose(inverse(mv));

    vertexPos = (mv * vec4(mdl_vertexPos, 1.0)).xyz;
    vertexNormal = normalize((normalMatrix * vec4(mdl_vertexNormal, 1.0)).xyz);

    gl_Position = mvp * vec4(mdl_vertexPos, 1.0);
}