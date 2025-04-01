namespace Engine.Rendering;

public class SpriteRenderer : Component, IRenderable
{
	private Transform? _transform;
	private SpriteManager? _manager;
	private Shader? _shader;

	protected override void OnCreate()
	{
		_manager = GlobalManager.Get<SpriteManager>();
		Renderer.Register(this);

		_shader = new Shader(
			"""
			#version 330 core

			layout (location = 0) in vec2 a_pos;
			layout (location = 1) in vec2 a_uv;

			out vec2 v_uv;

			void main()
			{
				gl_Position = vec4(a_pos, 0.0, 1.0);
				v_uv = a_uv;
			}
			""",
			"""
			#version 330 core

			in vec2 v_uv;
			out vec4 f_color;

			void main()
			{
				f_color = vec4(1.0, 0.0, 0.0, 1.0);
			}
			"""
			);
			
	}

	protected override void OnDestory()
	{
		Renderer.Unregister(this);
		_shader!.Destroy();
	}

	public RenderLayer GetRenderLayer()
	{
		return RenderLayer.Default;
	}

	public int Render()
	{
		_manager!.BindSpriteMesh();
		_shader!.Bind();

		return _manager!.Indices.Length;
	}
}
