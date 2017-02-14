#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform vec3 u_cameraPosition;

struct Material {
    vec4 baseColor;
    
    bool hasTexture;
    sampler2D texture;
    
    float specularPower;
    float specularIntensity;
};

uniform Material u_material;

in vec3 vs_vertexPos;
in vec3 vs_worldPos;
in vec3 vs_normal;
in vec2 vs_texCoord;

layout(location = 0) out vec4 fs_diffuseColor;
layout(location = 1) out vec3 fs_worldPos;
layout(location = 2) out vec3 fs_normal;
layout(location = 3) out vec3 fs_texCoord;
layout(location = 4) out vec2 fs_specular;

void main()
{
    vec4 diffuseColor = u_material.baseColor;

    fs_worldPos = vs_worldPos;


    if(u_material.hasTexture)
        diffuseColor *= texture2D(u_material.texture, vs_texCoord);

    fs_diffuseColor = diffuseColor;
    fs_normal = vs_normal;
    fs_texCoord = vec3(vs_texCoord, 0);
    fs_specular = vec2(u_material.specularPower, u_material.specularIntensity);
}
