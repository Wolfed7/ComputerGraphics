#version 460 core
out vec4 FragColor;
in vec4 vColor;
uniform vec4 color;
uniform vec4 innerColor;

void main()
{
   FragColor = vColor * color * innerColor;
}