#version 440 core

out vec4 FragColor;

uniform float time;

in vec2 UV0;
in vec3 Normals;
in vec3 FragPos;

void main()
{
    const float m = 10;
    const float fuzziness = 0.5f;
    const float hardness = 0.2;
    float r = hardness + fuzziness * sin(UV0.x * m + sin(UV0.y * m + time *10) + time);


    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
}