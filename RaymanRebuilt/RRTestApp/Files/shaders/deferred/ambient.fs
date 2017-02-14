#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform sampler2D u_diffuseColor;

uniform vec3 u_ambientLight;

in vec2 vs_texCoord;

out vec4 fs_color;

void main()
{
    fs_color = texture2D(u_diffuseColor, vs_texCoord) * vec4(u_ambientLight, 1);
}
