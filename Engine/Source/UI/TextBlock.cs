using Engine.Rendering;
using OpenTK.Mathematics;
using System.Text;

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
	public Color4 Color { get; set; } = Color4.White;

	private bool _shouldRegenerate;
	private string _text = string.Empty;

	private Shader? _shader;
	private Texture? _fontTexture;
	private readonly DynamicMeshBatch _mesh = new();

	public TextBlock() { }
	public TextBlock(string text)
	{
		Text = text;
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		Renderer.RegisterHook(RenderHook);

		_shader = Resource.LoadEngineResource<Shader>("DefaultFont.shader");
		_fontTexture = Resource.LoadEngineResource<Texture>("DefaultFont.bmp");

		RegenerateTextMesh();
	}

	protected override UIRect CalculateRect(UIEntity child)
	{
		return base.CalculateRect(child);
	}

	private void RegenerateTextMesh()
	{
		_text = _text.ToUpper();
		var builder = MeshBatchBuilder.Create();

		for (int i = 0; i < _text.Length; i++)
		{
			char c = _text[i];
			//int index; // Value of 0 equals first character in font texture.

			var bytes = Encoding.ASCII.GetBytes([c]);
			byte bCode = bytes[0];
			Log.Assert(bCode is >= 32 and <= 137, $"Text block text character '{c}' is out of the allowed ASCII range!");

			// Turn 32-137 -> 0-105.
			int code = bCode - 32;

			float cellSize = 1f / 8f;

			int x = code % 8;
			int y = code / 8;

			Vector2 offset = new Vector2(x, 1f - y) * cellSize;
			offset.Y -= (cellSize * 2f) - 0.01f;
			Vector2 scale = Vector2.One / 8f;

			// TODO: Calculate offset and scale of uv.

			builder.Quad(new(i * 0.2f, 0f), Vector2.One * 0.15f, offset, scale);
		}

		builder.Build(out float[] vertices, out uint[] indices);
		_mesh.Update(vertices, indices);
	}

	protected override int OnRender()
	{
		if (_shouldRegenerate)
		{
			_shouldRegenerate = false;
			RegenerateTextMesh();
		}

		_fontTexture!.Bind();

		_shader!.Bind();
		_shader!.SetUniform("u_color", Color);
		_shader!.SetUniform("u_mvp", Camera.Active.GetProjectionMatrix());

		_mesh!.Bind();
		return _mesh.IndexCount;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		_mesh.Destroy();
		_shader!.Release();
		_fontTexture!.Release();
	}
}
