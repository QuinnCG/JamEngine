// Vertex
#version 330 core

layout (location = 0) in vec2 a_pos;
layout (location = 1) in vec2 a_uv;

out vec2 v_uv;

uniform mat4 u_mvp;

void main()
{
	gl_Position = vec4(a_pos, 0.0, 1.0) * u_mvp;
	v_uv = a_uv;
}

// Fragment
#version 330 core

in vec2 v_uv;
out vec4 f_color;

uniform vec4 u_color;

uniform bool u_useTex = false;
uniform sampler2D u_tex;

void main()
{
	if (u_useTex)
	{
		f_color = u_color * texture(u_tex, v_uv);
	}
	else
	{
		f_color = u_color;
	}
}
