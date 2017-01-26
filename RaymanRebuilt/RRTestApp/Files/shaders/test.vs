#version 120

varying vec3 vertex_color;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
    vertex_color = gl_Vertex.xyz;
}