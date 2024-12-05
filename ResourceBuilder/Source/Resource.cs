namespace ResourceBuilder;

record Resource(string Name, byte[] Data)
{
	public string Name { get; } = Name;
	public byte[] Data { get; } = Data;

	/// <summary>
	/// Set after serializing resources.
	/// <br>Used in the construction of the index file.</br>
	/// </summary>
	public long FileIndex { get; set; }

	public static void Encrypt(byte[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] += 100;
		}
	}

	public ReadOnlySpan<byte> GetRaw()
	{
		// DATA SIZE
		// DATA

		byte[] dataSize = BitConverter.GetBytes(Data.Length);

		byte[] raw = [.. dataSize, .. Data];
		Encrypt(raw);

		return new ReadOnlySpan<byte>(raw);
	}
}
