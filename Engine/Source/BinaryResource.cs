namespace Engine;

public class BinaryResource : Resource
{
	public byte[] Data { get; private set; } = [];

	protected override void OnLoad(byte[] data)
	{
		Data = data;
	}

	protected override void OnFree()
	{
		Data = [];
	}

	protected override byte[] OnSave()
	{
		return Data;
	}
}
