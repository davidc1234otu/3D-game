#version 440 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormals;
layout (location = 2) in vec2 UVs;

out vec2 UV0;
out vec3 Normals;
out vec3 FragPos;

uniform float time;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    vec4 pos = vec4(aPosition, 1.0);

    const float m = 2;
    const float speed = 5;
    const float fuzziness = 0.2f;
    pos.z += fuzziness * sin(UVs.x * m + UVs.y * m + time * speed);

    gl_Position = pos * model * view * projection;
    UV0 = UVs;
    Normals = aNormals * mat3(transpose(inverse(model)));
    FragPos = vec3(pos * model);
} 