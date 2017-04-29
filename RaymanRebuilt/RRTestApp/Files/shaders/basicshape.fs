#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

struct Material {
    vec4 baseColor;
    
    bool hasTexture;
    sampler2D texture;

    bool shaded;
};

uniform Material u_material;

in vec3 vs_vertexPos;
in vec3 vs_worldPos;
in vec3 vs_normal;
in vec2 vs_texCoord;
in mat4 vs_normalMatrix;

out vec4 fs_diffuseColor;

void main()
{
    vec4 totalColor = u_material.baseColor;

    if(u_material.hasTexture)
        totalColor *= texture2D(u_material.texture, vs_texCoord);

    fs_diffuseColor = totalColor;
}
