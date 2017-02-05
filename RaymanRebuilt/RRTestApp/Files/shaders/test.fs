#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

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

in vec3 vertexPos;
in vec3 vertexNormal;
in vec2 vertexTexCoord;

out vec4 gl_FragColor;

vec3 lightPos = vec3(0, 10, 0);

vec4 calculateDirectionalLight() {
    float angleCos = dot(-u_directionalLight.direction, vertexNormal);

    if(angleCos < 0)
        return vec4(0, 0, 0, 1);

    return vec4(u_directionalLight.base.color * u_directionalLight.base.intensity * angleCos, 1);
}

vec4 calculateSpecular(BaseLight baseLight, vec3 direction) {
    float angleCos = dot(-direction, vertexNormal);

    if(angleCos < 0)
        return vec4(0, 0, 0, 1);

    return vec4(baseLight.color * pow(angleCos, u_material.specularPower) * u_material.specularIntensity, 1);
}

void main()
{
    vec4 materialColor = u_material.baseColor;

    if(u_material.hasTexture)
        materialColor *= texture2D(u_material.texture, vertexTexCoord);

    vec4 dirLightColor = calculateDirectionalLight();
    vec4 specularColor = calculateSpecular(u_directionalLight.base, u_directionalLight.direction);

    vec4 totalColor = materialColor * (dirLightColor + vec4(u_ambientLight, 1)) + specularColor;
    totalColor.w = u_material.baseColor.w * materialColor.w;

    gl_FragColor = totalColor;
}
