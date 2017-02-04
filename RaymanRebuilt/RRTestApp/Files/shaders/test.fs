#version 330

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform vec3 u_ambientLight;

struct Material {
    bool hasTexture;
    sampler2D texture;
    vec4 baseColor;
};

uniform Material u_material;

in vec3 vertexPos;
in vec3 vertexNormal;
in vec2 vertexTexCoord;

out vec4 gl_FragColor;

vec3 lightPos = vec3(0, 10, 0);

void main()
{
    vec4 texColor = u_material.baseColor;

    float lightValue = clamp(dot(vertexNormal, normalize((u_viewMatrix * vec4(lightPos, 1)).xyz - vertexPos)), 0, 1);

    //lightValue = (3.14159265 - acos(lightValue)) / 3.14159265;

    if(u_material.hasTexture) {
        texColor *= texture2D(u_material.texture, vertexTexCoord);
    }

    vec4 totalColor = texColor;
    totalColor *= lightValue;
    totalColor += pow(lightValue, 10);
    totalColor += texColor * vec4(u_ambientLight, 1);
    totalColor.w = u_material.baseColor.w * texColor.w;

    gl_FragColor = totalColor;
}
