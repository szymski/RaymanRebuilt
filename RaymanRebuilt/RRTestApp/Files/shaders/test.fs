#version 120

varying vec3 vertex_color;

void main()
{
    gl_FragColor = vec4(sin(vertex_color.x), sin(vertex_color.y), sin(vertex_color.z), 1.0);
}
