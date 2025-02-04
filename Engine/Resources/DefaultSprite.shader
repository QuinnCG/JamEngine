// Vertex
#version 330 core

layout (location = 0) in vec2 a_pos;
layout (location = 1) in vec2 a_uv;

out vec2 v_uv;

uniform mat4 u_mvp;

void main()
{
	v_uv = a_uv;
	gl_Position = vec4(a_pos, 0.0, 1.0) * u_mvp;
}

// Fragment
#version 330 core

in vec2 v_uv;
out vec4 f_color;

uniform vec4 u_color;
uniform bool u_isTextured;

uniform sampler2D u_texture;

void main()
{
	if (u_isTextured)
	{
		f_color = u_color * texture(u_texture, v_uv);
	}
	else
	{
		f_color = u_color;
	}
}
