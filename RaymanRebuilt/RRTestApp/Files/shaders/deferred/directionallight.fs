#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform sampler2D u_worldPos;
uniform sampler2D u_diffuseColor;
uniform sampler2D u_normal;
uniform sampler2D u_specular;
uniform vec3 u_cameraPosition;

struct BaseLight {
    vec3 color;
    float intensity;
};

struct DirectionalLight {
    BaseLight base;
    vec3 direction;
};

uniform DirectionalLight u_directionalLight;

in vec2 vs_texCoord;

out vec4 fs_color;

vec3 calculateLight(BaseLight base, vec3 direction) {
    vec2 specularValues = texture2D(u_specular, vs_texCoord).xy;
    float specularPower = specularValues.x;
    float specularIntensity = specularValues.y;

    vec3 vertexNormal = texture2D(u_normal, vs_texCoord).xyz;

    if(base.intensity == 0)
        return vec3(0, 0, 0);

    float diffuseFactor = dot(vertexNormal, -direction);

    vec3 diffuseColor = vec3(0, 0, 0);
    vec3 specularColor = vec3(0, 0, 0);

    if(diffuseFactor > 0) {
        // Diffuse

        diffuseColor = base.color * base.intensity * diffuseFactor;

        // Specular

        vec3 directionToEye = normalize(u_cameraPosition - texture2D(u_worldPos, vs_texCoord).xyz);
        vec3 directionReflect = normalize(reflect(direction, vertexNormal));

        float specularFactor = dot(directionToEye, directionReflect);

        if(specularFactor > 0)
            specularColor = base.color * base.intensity * pow(specularFactor, specularPower) * specularIntensity;
    }

    return diffuseColor + specularColor;
}

void main()
{
    fs_color = texture2D(u_diffuseColor, vs_texCoord) * vec4(calculateLight(u_directionalLight.base, u_directionalLight.direction), 1.0);
}
