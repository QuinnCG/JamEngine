using System.Text;

namespace ResourceBuilder;

static class ResourceBuilder
{
	private const string SourcePath = "../../../../Sandbox/Resources/";
	private const string DestPath = "../../../../Sandbox/bin/Debug/net9.0/";

	static void Main()
	{
		// res.dat
		var fs = File.Create(DestPath + "res.dat");
		var resources = EnumerateDirectory(SourcePath);

		foreach (var resource in resources)
		{
			resource.FileIndex = fs.Position;
			fs.Write(resource.GetRaw());
		}

		// index.dat
		fs.Close();
		fs = File.Create(DestPath + "index.dat");

		byte[] table = CreateTable(resources);
		fs.Write(table, 0, table.Length);

		fs.Close();
	}

	static List<Resource> EnumerateDirectory(string path)
	{
		var resources = new List<Resource>();
		var subFiles = Directory.EnumerateFiles(path);

		foreach (var file in subFiles)
		{
			const string key = "Resources/";
			int startOfNameIndex = file.IndexOf(key) + key.Length;

			string name = file[startOfNameIndex..];
			byte[] data = File.ReadAllBytes(file);

			var res = new Resource(name, data);
			resources.Add(res);
		}

		var subDirs = Directory.EnumerateDirectories(path);

		foreach (var dir in subDirs)
		{
			var result = EnumerateDirectory(dir);
			resources.AddRange(result);
		}

		return resources;
	}

	static byte[] CreateTable(IEnumerable<Resource> resources)
	{
		// Separate file that references locations in "res.dat".

		// ITEM 1 NAME SIZE			4 bytes
		// ITEM 1 NAME				variable
		// ITEM 1 ENTRY INDEX		8 bytes
		// (repeated)

		var bytes = new List<byte>();

		foreach (var resource in resources)
		{
			byte[] nameSizeBytes = BitConverter.GetBytes(resource.Name.Length);
			byte[] nameBytes = Encoding.Default.GetBytes(resource.Name);
			byte[] index = BitConverter.GetBytes(resource.FileIndex);

			bytes.AddRange(nameSizeBytes);
			bytes.AddRange(nameBytes);
			bytes.AddRange(index);
		}

		byte[] byteArray = [.. bytes];
		Resource.Encrypt(byteArray);

		return byteArray;
	}
}
