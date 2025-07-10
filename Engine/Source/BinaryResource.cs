namespace Engine;

public class BinaryResource : Resource, ISaveable
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

	public void Save()
	{
		File.WriteAllBytes(FullPath, Data);
	}
}
