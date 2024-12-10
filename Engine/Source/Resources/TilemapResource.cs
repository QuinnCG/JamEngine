using System.Text;

namespace Engine.Resources;

public record TilemapResource : IResource
{
	// HACK: Use IResourceLoader ideally, but currently it still requires a file on disk; it seems.
	// TODO: Support non-embedded tilesets.

	public string Data { get; private set; } = string.Empty;

	~TilemapResource()
	{
		Dispose();
	}

	public void Load(byte[] data)
	{
		Data = Encoding.Default.GetString(data);
	}

	public bool IsLoaded()
	{
		return !string.IsNullOrWhiteSpace(Data);
	}

	public void Dispose()
	{
		Data = string.Empty;
		GC.SuppressFinalize(this);
	}
}
