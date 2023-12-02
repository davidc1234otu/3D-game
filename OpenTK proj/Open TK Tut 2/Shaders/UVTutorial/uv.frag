#version 440 core

out vec4 FragColor;

in vec2 UV0;
in vec3 Normals;
in vec3 FragPos;

void main()
{
    FragColor = vec4(UV0.x, UV0.y, 0.0, 1.0);
}