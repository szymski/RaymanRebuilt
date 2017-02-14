#version 330

layout(location = 0) in vec3 mdl_vertexPos;
layout(location = 1) in vec3 mdl_vertexNormal;
layout(location = 2) in vec2 mdl_vertexTexCoord;

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

out vec2 vertexTexCoord;

void main() {
    mat4 mvp = u_projectionMatrix * u_viewMatrix * u_modelMatrix;
    gl_Position = mvp * vec4(mdl_vertexPos, 1);
    vertexTexCoord = mdl_vertexTexCoord;
}