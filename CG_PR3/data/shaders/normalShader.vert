#version 330 core
layout (location = 0) in vec3 nPos;

uniform mat4 nmodel;
uniform mat4 nview;
uniform mat4 nprojection;

void main()
{
    gl_Position = vec4(nPos, 1.0) * nmodel * nview * nprojection;
}