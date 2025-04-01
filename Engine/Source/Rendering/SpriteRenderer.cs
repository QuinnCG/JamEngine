namespace Engine.Rendering;

public class SpriteRenderer : Component, IRenderable
{
	private Transform? _transform;
	private SpriteManager? _manager;
	private Shader? _shader;

	protected override void OnCreate()
	{
		_manager = GlobalManager.Get<SpriteManager>();
		_shader = Resource.LoadEngineResource<Shader>("DefaultSprite.shader");

		Renderer.Register(this);
			
	}

	protected override void OnDestory()
	{
		Renderer.Unregister(this);
		_shader!.Release();
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
