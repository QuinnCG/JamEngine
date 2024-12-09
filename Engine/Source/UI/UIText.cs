using Engine.Rendering;
using OpenTK.Mathematics;

namespace Engine.UI;

public class UIText : UIEntity
{
	// TODO: Implement UI text.
	// Create own mesh data unique to each instance.
	// Update mesh data when text changes.

	// TODO: Support default font fallback.

	public string Text { get; set; }
	public Texture Font { get; set; }

	private readonly Mesh _mesh;

	public UIText(Texture font)
		: this(font, string.Empty) { }
	public UIText(Texture font, string text)
		: base(Color4.White)
	{
		Font = font;
		Text = text;

		var builder = new MeshBuilder()
			.Quad(new Vector2(0f, 1f), 0f, Vector2.One * 0.5f)
			.Quad(new Vector2(1f, 1f), 0f, Vector2.One * 0.5f);

		_mesh = builder.Build(isStatic: false);
	}

	protected override int GetIndexCount()
	{
		return _mesh.IndexCount;
	}

	protected override void OnBind()
	{
		_mesh.Bind();
		Sprite?.Texture.Bind();
	}

	protected override void OnUpdate()
	{
		
	}
}
