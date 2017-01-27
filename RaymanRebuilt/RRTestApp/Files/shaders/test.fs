#version 330

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

in vec3 vertexPos;
in vec3 vertexNormal;

out vec4 gl_FragColor;

vec3 lightPos = vec3(0.0, 0.0, 0.0);

void main()
{
    float lightDot = dot(vertexNormal, normalize((viewMatrix * vec4(lightPos, 1)).xyz - vertexPos));

    gl_FragColor = vec4(lightDot, 0, 0, 1);
}
