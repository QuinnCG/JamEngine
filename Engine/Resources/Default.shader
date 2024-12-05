// Vertex
#version 330 core

layout (location = 0) in vec2 a_pos;

void main()
{
	gl_Position = vec4(a_pos, 0.0, 1.0);
}

// Fragment
#version 330 core

layout (location = 0) out vec4 f_color;

uniform vec4 u_tint;

void main()
{
	f_color = u_tint;
}
