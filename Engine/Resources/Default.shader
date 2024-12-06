// Vertex
#version 330 core

layout (location = 0) in vec2 a_position;
layout (location = 1) in vec2 a_uv;

out vec2 v_uv;

uniform mat4 u_mvp;

uniform vec2 u_UVOffset;
uniform vec2 u_UVScale;

void main()
{
	v_uv = (a_uv * u_UVScale) + u_UVOffset;
	gl_Position = vec4(a_position, 0.0, 1.0) * u_mvp;
}

// Fragment
#version 330 core

in vec2 v_uv;
out vec4 f_color;

uniform vec4 u_tint;
uniform sampler2D u_texture;

void main()
{
	f_color = u_tint * texture(u_texture, v_uv);
}
