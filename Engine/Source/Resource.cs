using System.Text;

namespace Engine;

public static class Resource
{
	private static readonly Dictionary<string, long> _nameToIndex = [];

	// TODO: Only use binary data for release mode. In debug mode, use the resource files directly (have them copied over in debug mode).

	public static T Load<T>(string name) where T : IResource, new()
	{
		name = name.Replace('/', '\\');

		if (!_nameToIndex.TryGetValue(name, out long fileIndex))
		{
			throw new InvalidOperationException($"Failed to find a resource with the name '{name}'!");
		}

		using var fs = File.OpenRead("res.dat");
		fs.Position = fileIndex;

		var dataSizeBytes = new byte[4];
		fs.ReadExactly(dataSizeBytes, 0, 4);

		Decrypt(dataSizeBytes);
		int dataSize = BitConverter.ToInt32(dataSizeBytes, 0);

		var data = new byte[dataSize];
		fs.ReadExactly(data, 0, dataSize);
		Decrypt(data);

		var res = new T();
		res.Load(data);

		return res;
	}

	internal static void Initialize()
	{
		using var fs = File.OpenRead("index.dat");
		using var reader = new BinaryReader(fs);

		while (fs.Position < fs.Length)
		{
			int nameSize = BitConverter.ToInt32(Decrypt(reader.ReadBytes(4)), 0);
			string name = Encoding.Default.GetString(Decrypt(reader.ReadBytes(nameSize)));
			long dataIndex = BitConverter.ToInt64(Decrypt(reader.ReadBytes(8)), 0);

			_nameToIndex.Add(name, dataIndex);
		}
	}

	/// <summary>
	/// Gets an engine embedded resource as a stream.
	/// <br>The path is relative to Engine/Resources.</br>
	/// </summary>
	/// <returns>A stream of the returned resource or null if it wasn't found.
	/// <br>Remeber to dispose of the stream.</br></returns>
	internal static Stream? LoadFromEngine(string path)
	{
		path = $"Engine.Resources.{path}";
		return typeof(Resource).Assembly.GetManifestResourceStream(path);
	}

	private static byte[] Decrypt(byte[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] -= 100;
		}

		return data;
	}
}
