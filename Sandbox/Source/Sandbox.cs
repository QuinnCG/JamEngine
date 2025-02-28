using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var saveFile = new SaveFile()
		{
			new() { Type = typeof(MyEntity), Entity = new MyEntity() },
			new() { Type = typeof(MyOtherEntity), Entity = new MyOtherEntity() },
		};

		string json = JsonSerializer.Serialize(saveFile);
		File.WriteAllText("Save.json", json);

		// Deserialize.

		Console.WriteLine(File.ReadAllText("Save.json"));
		Console.ReadKey(true);
	}
}

class SaveFileConverter : JsonConverter<SaveFile>
{
	public override SaveFile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var saveFile = new SaveFile();
		
		return saveFile;
	}
	public override void Write(Utf8JsonWriter writer, SaveFile value, JsonSerializerOptions options)
	{
		
	}
}

record SaveFile : IEnumerable<SaveEntry>
{
	public readonly List<SaveEntry> Entries = [];

	public void Add(SaveEntry entry)
	{
		Entries.Add(entry);
	}

	public IEnumerator<SaveEntry> GetEnumerator()
	{
		return Entries.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Entries.GetEnumerator();
	}
}

[JsonSerializable(typeof(SaveEntry))]
record SaveEntry
{
	public string DummyString = Random.Shared.NextSingle().ToString();
	public required Type Type;
	public required Entity Entity;
}

class Entity
{
	public Guid GUID = Guid.NewGuid();
}

class MyEntity : Entity
{
	public string MyText = Random.Shared.NextSingle().ToString();
}

class MyOtherEntity : Entity
{
	public float MyFloat = Random.Shared.NextSingle() * 1000f;
}
