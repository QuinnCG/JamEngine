using System.Text;

namespace Engine;

public class TextResource : Resource
{
	public string Text => _text!;
	private string? _text;

	internal TextResource() { }

	protected override void OnLoad(byte[] data)
	{
		_text = Encoding.Default.GetString(data);
	}

	protected override void OnFree()
	{
		_text = null;
	}
}
