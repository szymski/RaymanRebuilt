#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform sampler2D u_worldPos;
uniform sampler2D u_diffuseColor;
uniform sampler2D u_normal;
uniform sampler2D u_specular;
uniform vec3 u_cameraPosition;

uniform vec3 u_ambientLight;

in vec2 vs_texCoord;

out vec4 fs_color;

void main()
{
    fs_color = u_ambientLight;
}
