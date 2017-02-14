#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform vec3 u_cameraPosition;

uniform vec3 u_ambientLight;

struct Material {
    vec4 baseColor;
    
    bool hasTexture;
    sampler2D texture;
    
    float specularPower;
    float specularIntensity;
};

uniform Material u_material;

struct BaseLight {
    vec3 color;
    float intensity;
};

struct DirectionalLight {
    BaseLight base;
    vec3 direction;
};

uniform DirectionalLight u_directionalLight;

struct Attenuation {
    float constant;
    float linear;
    float exponential;
};

struct PointLight {
    BaseLight base;
    Attenuation attenuation;
    vec3 position;
};

#define MAX_POINT_LIGHTS 8

uniform PointLight u_pointLights[MAX_POINT_LIGHTS];

uniform samplerCube u_cubemapTexture;

in vec3 vertexPos;
in vec3 worldVertexPos;
in vec3 vertexNormal;
in vec2 vertexTexCoord;

out vec4 gl_FragColor;

vec3 lightPos = vec3(0, 10, 0);

// vec4 calculateSpecular(BaseLight baseLight, vec3 direction) {
//     vec3 directionToEye = normalize(u_cameraPosition - worldVertexPos);
//     vec3 directionReflect = normalize(reflect(direction, vertexNormal));
//     float angleCos = dot(directionToEye, directionReflect);

//     if(angleCos < 0)
//         return vec4(0, 0, 0, 1);

//     return vec4(baseLight.color * baseLight.intensity * pow(angleCos, u_material.specularPower) * u_material.specularIntensity, 1);
// }

vec3 calculateLight(BaseLight base, vec3 direction) {
    if(base.intensity == 0)
        return vec3(0, 0, 0);

    float diffuseFactor = dot(vertexNormal, -direction);

    vec3 diffuseColor = vec3(0, 0, 0);
    vec3 specularColor = vec3(0, 0, 0);

    if(diffuseFactor > 0) {
        // Diffuse

        diffuseColor = base.color * base.intensity * diffuseFactor;

        // Specular

        vec3 directionToEye = normalize(u_cameraPosition - worldVertexPos);
        vec3 directionReflect = normalize(reflect(direction, vertexNormal));

        float specularFactor = dot(directionToEye, directionReflect);

        if(specularFactor > 0)
            specularColor = base.color * base.intensity * pow(specularFactor, u_material.specularPower) * u_material.specularIntensity;
    }

    return diffuseColor + specularColor;
}

vec3 calculateDirectionalLight() {
    return calculateLight(u_directionalLight.base, u_directionalLight.direction);
}

void main()
{
    vec4 materialColor = u_material.baseColor;

    if(u_material.hasTexture)
        materialColor *= texture2D(u_material.texture, vertexTexCoord);

    vec4 dirLightColor = vec4(calculateDirectionalLight(), 1);

    vec3 pointLightsColor = vec3(0, 0, 0);
    for(int i = 0; i < MAX_POINT_LIGHTS; i++) {
        PointLight light = u_pointLights[i];

        if(light.base.intensity == 0)
            continue;

        float dist = length(light.position - worldVertexPos);
        float attenuation = 1.0 / (1 + light.attenuation.constant + light.attenuation.linear * dist + light.attenuation.exponential * dist * dist);
        
        pointLightsColor += calculateLight(light.base, normalize(worldVertexPos - light.position)) * attenuation;
    }

    vec3 directionToEye = -normalize(u_cameraPosition - worldVertexPos);
    vec4 cubemapColor = texture(u_cubemapTexture, normalize(reflect(directionToEye, vertexNormal)));

    vec4 totalColor = materialColor * (dirLightColor + vec4(u_ambientLight, 1) + vec4(pointLightsColor, 1)) + cubemapColor * 0.4f;
    totalColor.w = u_material.baseColor.w * materialColor.w;

    gl_FragColor = totalColor;
}
