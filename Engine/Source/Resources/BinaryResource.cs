namespace Engine.Resources;

public record BinaryResource : IResource
{
	public byte[] Data { get; private set; } = [];

	public void Load(byte[] data)
	{
		Data = data;
	}

	public void Dispose()
	{
		Data = [];
		GC.SuppressFinalize(this);
	}
}
