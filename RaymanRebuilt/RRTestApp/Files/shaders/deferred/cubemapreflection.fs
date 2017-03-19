#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform sampler2D u_worldPos;
uniform sampler2D u_diffuseColor;
uniform sampler2D u_normal;
uniform sampler2D u_specular;
uniform vec3 u_cameraPosition;

uniform samplerCube u_cubemap;

in vec2 vs_texCoord;

out vec4 fs_color;

void main()
{
    if(texture2D(u_diffuseColor, vs_texCoord).a == 0) {
        fs_color = vec4(0.0);
        return;
    }

    vec3 directionToEye = -normalize(u_cameraPosition - texture2D(u_worldPos, vs_texCoord).xyz);
    vec3 vertexNormal = texture2D(u_normal, vs_texCoord).xyz;

    vec3 cubemapColor = texture(u_cubemap, normalize(reflect(directionToEye, vertexNormal))).xyz;

    fs_color = vec4(cubemapColor * texture2D(u_specular, vs_texCoord).y * 0.4, 1.0);
}
