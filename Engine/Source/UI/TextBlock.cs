using Engine.Rendering;
using OpenTK.Mathematics;

namespace Engine.UI;

public class TextBlock : UIEntity
{
	public string Text
	{
		get => _text;
		set
		{
			_text = value;
			_shouldRegenerate = true;
		}
	}

	private bool _shouldRegenerate;
	private string _text = string.Empty;

	private Shader? _shader;
	private Texture? _fontTexture;
	private readonly DynamicMeshBatch _mesh = new();
	private RenderHook? _hook;

	protected override void OnCreate()
	{
		_shader = Resource.LoadEngineResource<Shader>("DefaultFont.shader");
		_fontTexture = Resource.LoadEngineResource<Texture>("DefaultFont.bmp");
		_hook = new RenderHook(OnRender, GetRenderLayer);

		Renderer.RegisterHook(_hook);
		Regenerate();
	}

	protected override void OnUpdate()
	{
		if (_shouldRegenerate)
		{
			_shouldRegenerate = false;
			Regenerate();
		}
	}

	protected override void OnDestroy()
	{
		Renderer.UnregisterHook(_hook!);

		_mesh.Destroy();
		_shader!.Release();
		_fontTexture!.Release();
	}

	private void Regenerate()
	{
		var builder = MeshBatchBuilder.Create();

		for (int i = 0; i < _text.Length; i++)
		{
			char c = _text[i];

			Vector2 offset = Vector2.Zero;
			Vector2 scale = Vector2.One / 8f;

			// TODO: Calculate offset and scale of uv.

			builder.Quad(new(i * 0.2f, 0f), Vector2.One * 0.15f, offset, scale);
		}

		builder.Build(out float[] vertices, out uint[] indices);
		_mesh.Update(vertices, indices);
	}

	private int OnRender()
	{
		_fontTexture!.Bind();

		_shader!.Bind();
		_shader!.SetUniform("u_color", Color4.White);

		_mesh!.Bind();

		return _mesh.IndexCount;
	}

	private RenderLayer GetRenderLayer()
	{
		return RenderLayer.Default;
	}
}
