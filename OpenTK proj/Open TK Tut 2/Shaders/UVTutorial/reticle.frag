#version 440 core

out vec4 FragColor;

uniform float aspectRatio; // width/height;

in vec2 UV0;
in vec3 Normals;
in vec3 FragPos;

void main()
{
    const vec2 midPoint = vec2(0.5f * aspectRatio, 0.5f);

    vec2 tempUVs = UV0;
    tempUVs.x *= aspectRatio;

    
    float dist = distance(midPoint, tempUVs);

    const float reticleWidth = 0.01f;
    const float innerThickness = 0.005f;

    dist = step(reticleWidth, dist) - step(reticleWidth + innerThickness, dist);

    if(abs(midPoint.x - tempUVs.x) < innerThickness || abs(midPoint.y - tempUVs.y) < innerThickness)
    {
        dist = 0;
    }

    FragColor = dist * vec4(1,0,0,1);
}