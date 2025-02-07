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
uniform sampler2D u_texture;

void main()
{
	f_color = u_color * texture(u_texture, v_uv);

	if (f_color.x < 0.1 && f_color.y < 0.1 && f_color.z < 0.1)
	{
		discard;
	}
}
