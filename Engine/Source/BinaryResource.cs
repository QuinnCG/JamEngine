namespace Engine;

public class BinaryResource : Resource
{
	public byte[] Data => _data!;
	private byte[]? _data;

	protected override void OnLoad(byte[] data)
	{
		_data = data;
	}

	protected override void OnFree()
	{
		_data = null;
	}
}
