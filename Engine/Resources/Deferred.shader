// Vertex
#version 330 core

layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aUV;

out vec2 vUV;

void main()
{
	gl_Position = vec4(aPos, 0.0, 1.0);
	vUV = aUV;
}

// Fragment
#version 330 core

in vec2 vUV;
out vec4 fColor;

uniform sampler2D uTexture;

void main()
{
	fColor = texture(uTexture, vUV);
}
