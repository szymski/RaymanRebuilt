#version 330

uniform sampler2D u_texture;

in vec2 vertexTexCoord;

out vec4 gl_FragColor;

void main() {
    gl_FragColor = texture2D(u_texture, vertexTexCoord);
}