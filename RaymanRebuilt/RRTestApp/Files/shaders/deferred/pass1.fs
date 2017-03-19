#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform vec3 u_cameraPosition;

struct Material {
    vec4 baseColor;
    
    bool hasDiffuseTexture;
    sampler2D diffuseTexture;
    
    bool hasNormalTexture;
    sampler2D normalTexture;
    
    float specularPower;
    float specularIntensity;
};

uniform Material u_material;

in vec3 vs_vertexPos;
in vec3 vs_worldPos;
in vec3 vs_normal;
in vec2 vs_texCoord;
in mat4 vs_normalMatrix;

layout(location = 0) out vec4 fs_diffuseColor;
layout(location = 1) out vec4 fs_worldPos;
layout(location = 2) out vec4 fs_normal;
layout(location = 3) out vec4 fs_texCoord;
layout(location = 4) out vec4 fs_specular;

void main()
{
    vec4 diffuseColor = u_material.baseColor;

    fs_worldPos = vec4(vs_worldPos, 1.0);

    if(u_material.hasDiffuseTexture)
        diffuseColor *= texture2D(u_material.diffuseTexture, vs_texCoord);

    fs_diffuseColor = diffuseColor;

    fs_normal = vec4(vs_normal, 1.0);

    if(u_material.hasNormalTexture)
        fs_normal = vec4(cross(fs_normal.xyz, 2.0 * texture2D(u_material.normalTexture, vs_texCoord).xyz - 1.0), 1.0);

    fs_texCoord = vec4(vs_texCoord, 0, 1.0);
    
    fs_specular = vec4(u_material.specularPower, u_material.specularIntensity, 0.0, 1.0);
}
