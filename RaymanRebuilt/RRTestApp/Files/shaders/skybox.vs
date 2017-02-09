#version 330

layout(location = 0) in vec3 mdl_vertexPos;

uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

out vec3 vertexTexCoord;

void main() {
    gl_Position = (u_projectionMatrix * vec4(mdl_vertexPos, 1)).xyww;
    vertexTexCoord = mdl_vertexPos * 0.0001 + (transpose(u_viewMatrix) * vec4(mdl_vertexPos, 1)).xyz;
}