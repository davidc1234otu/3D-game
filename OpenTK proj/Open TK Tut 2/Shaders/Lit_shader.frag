#version 440 core

out vec4 FragColor;

//in vec4 vertexColor; // Named the same

uniform vec4 uniformColor;

uniform sampler2D tex0;
uniform sampler2D tex1;

struct PointLight{
    vec3 lightColor;
    vec3 lightPosition;
    float lightIntensity;
};

uniform vec3 viewPos;
uniform PointLight pLights[100];
uniform int numPLights;

in vec2 UV0;
in vec3 Normals;
in vec3 FragPos;

vec3 HandleLighting()
{
    vec3 ADS;
    
    for(int i = 0; i < numPLights; ++i)
    {
        PointLight currentLight = pLights[i];
        
        float ambientStength = 0.1f;
        vec3 ambient = ambientStength * currentLight.lightColor;

        vec3 normals = normalize(Normals);
        vec3 lightDir = normalize(currentLight.lightPosition - FragPos);

        float diff = max(dot(normals, lightDir), 0);
        vec3 diffuse = diff * currentLight.lightColor;

        diffuse = vec3(min(diffuse.x, 1), min(diffuse.y,1), min(diffuse.z,1));

        const float shininess = 64;
        float specularStrength = 0.5f;
        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 reflectoinDir = reflect(-lightDir, normals);
        float spec = pow(max(dot(viewDir, reflectoinDir), 0), shininess);
        vec3 specularColor = specularStrength * spec * currentLight.lightColor;
        
        ADS += (ambient + diffuse + spec) * currentLight.lightIntensity;
    }
    return ADS;
}


void main()
{
    vec4 colorTex0 = texture(tex0, UV0);
    vec4 colorTex1 = texture(tex1, UV0);
    
    float threshhold = 0.9f;
    float average = (colorTex1.r + colorTex1.g + colorTex1.b)/3;
    
    if(average > threshhold)
    {
        colorTex1 = uniformColor;
    }
    
    FragColor = mix(colorTex0, colorTex1, colorTex1.a);
    
    
    
    FragColor = vec4(vec3(FragColor) * HandleLighting(), FragColor.a);
    //FragColor = vec4(vec3(FragColor) * (ambient + diffuse), FragColor.a);
}