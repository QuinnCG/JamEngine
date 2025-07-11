// Vertex
#version 330 core

layout (location = 0) in vec2 a_pos;
layout (location = 1) in vec2 a_uv;
layout (location = 2) in vec4 a_color;

out vec2 v_uv;
out vec4 v_color;

uniform mat4 u_mvp;

void main()
{
	gl_Position = vec4(a_pos, 0.0, 1.0) * u_mvp;

	v_uv = a_uv;
	v_color = a_color;
}

// Fragment
#version 330 core

in vec2 v_uv;
in vec4 v_color;

out vec4 f_color;

void main()
{
	f_color = v_color;
}
