#version 460 core
out vec4 FragColor;
in vec4 vColor;

void main()
{
   vec2 uv = gl_PointCoord - vec2(0.5);
   float dist = dot(uv, uv);
   float radius = 0.5;
   
   if (dist > radius * radius)
     discard;
   
   FragColor = vec4(vColor.rgb, 1.0 - dist / (radius * radius));
}