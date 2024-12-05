using System.Text;

namespace Engine.Resources;

public record TextResource : IResource
{
	public string Text { get; private set; } = string.Empty;
	public Encoding Encoding { get; init; } = Encoding.Default;

	public void Load(byte[] data)
	{
		Text = Encoding.GetString(data);
	}

	public void Dispose()
	{
		Text = string.Empty;
		GC.SuppressFinalize(this);
	}

	public override string ToString()
	{
		return Text;
	}
}
