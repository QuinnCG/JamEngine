using Engine.Rendering;
using OpenTK.Mathematics;
using System.Text;

namespace Engine.UI;

public class UIText : UIEntity
{
	// TODO: Implement UI text.
	// Create own mesh data unique to each instance.
	// Update mesh data when text changes.

	// TODO: Support default font fallback.

	public string Text
	{
		get => _text;
		set
		{
			_text = value;
			Regenerate();
		}
	}
	public Texture Font
	{
		get => Sprite!.Value.Texture;
		set => Sprite = new Sprite(value);
	}

	private string _text = string.Empty;
	private Mesh? _mesh;

	public UIText(Texture font)
		: this(font, string.Empty) { }
	public UIText(Texture font, string text)
		: base(Color4.White)
	{
		Font = font;
		_text = text;
	}

	protected override void OnCreate()
	{
		Regenerate();
		base.OnCreate();
	}

	protected override bool DiscardBlack()
	{
		return true;
	}

	protected override int GetIndexCount()
	{
		return _mesh!.IndexCount;
	}

	protected override void OnBind()
	{
		_mesh!.Bind();
		Sprite?.Texture.Bind();
	}

	private void Regenerate()
	{
		_mesh?.Dispose();

		var builder = new MeshBuilder();

		// Currently, only uppercase is supported.
		string text = _text.ToUpper();
		byte[] asciiBytes = Encoding.ASCII.GetBytes(text);

		// Each character will "step" this value up by its width.
		float totalXOffset = 0f;

		for (int i = 0; i < text.Length; i++)
		{
			// 'space' inclusive.
			const int minCode = 32;
			// '_' inclusive.
			const int maxCode = 95;

			int charAsciiIndex = asciiBytes[i];
			Log.Assert(charAsciiIndex is >= minCode and <= maxCode, $"Character '{Encoding.ASCII.GetString([asciiBytes[i]])}' is out of supported range!");

			// 0-63; to be usd for look-up in font bitmap.
			int charRelativeIndex = charAsciiIndex - minCode;

			// Columns in font bitmap.
			const int columCount = 8;
			// The actual index for the font bitmap.
			var imageIndex = new Vector2i(charRelativeIndex % columCount, charRelativeIndex / columCount);

			// Normalized size of cell in font bitmap; Columns == Rows
			float cellSize = 1f / columCount;

			var uvScale = new Vector2(cellSize);
			var uvOffset = (Vector2)imageIndex * cellSize;
			uvOffset.Y = 1f - uvOffset.Y - cellSize;

			var pos = Vector2.UnitX * totalXOffset;
			var scale = Vector2.One * 0.3f;

			float charWidth = 0.2f;
			totalXOffset += charWidth;

			var uvs = new Vector2[]
			{
				(new Vector2(0f, 0f) * uvScale) + uvOffset,
				(new Vector2(0f, 1f) * uvScale) + uvOffset,
				(new Vector2(1f, 1f) * uvScale) + uvOffset,
				(new Vector2(1f, 0f) * uvScale) + uvOffset
			};

			builder.Quad(pos, 0f, scale, uvs);
		}

		_mesh = builder.Build(isStatic: true);
	}
}
