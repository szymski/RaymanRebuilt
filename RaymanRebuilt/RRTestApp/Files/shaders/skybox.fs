#version 330

in vec3 vertexTexCoord;

uniform samplerCube u_cubemapTexture;

out vec4 gl_FragColor;

void main() {
    gl_FragColor = texture(u_cubemapTexture, vertexTexCoord);
}