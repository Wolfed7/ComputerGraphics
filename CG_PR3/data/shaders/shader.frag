#version 330 core
out vec4 FragColor;

uniform vec3 diffuse;

void main()
{
    FragColor = vec4(diffuse, 1.0); // set all 4 vector values to 1.0
}

//#version 330 core
//uniform float u_time;
//vec2 u_resolution;
//out vec4 FragColor;
//
//vec3 spherePosition = vec3(0.0, 0.0, 0.0);
//float sphereRadius = 0.3;
//
//void main() 
//{
//    vec3 surfacePosition = gl_FragCoord.xyz;
//    float fdistance = distance(surfacePosition, spherePosition);
//
//    if (fdistance <= sphereRadius) 
//	{
//        FragColor = vec4(1.0, 1.0, 1.0, 1.0); // white color for inside the sphere
//    } 
//	else 
//	{
//        FragColor = vec4(0.0, 0.0, 0.0, 1.0); // black color for outside the sphere
//    }
//}
//