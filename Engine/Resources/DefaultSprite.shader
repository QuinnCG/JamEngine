// Vertex
#version 330 core

layout (location = 0) in vec2 a_pos;
layout (location = 1) in vec2 a_uv;

out vec2 v_uv;

void main()
{
	gl_Position = vec4(a_pos, 0.0, 1.0);
	v_uv = a_uv;
}

// Fragment
#version 330 core

in vec2 v_uv;
out vec4 f_color;

void main()
{
	f_color = vec4(1.0, 0.0, 0.0, 1.0);
}
