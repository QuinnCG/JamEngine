using System.Reflection;

namespace Engine;

public static class Resource
{
	private static Assembly? _clientAssembly, _engineAssembly;
	private static string? _clientName;

	public static T Load<T>(string path) where T : IResource, new()
	{
		path = FormatPath(path);

		Log.Assert(_clientName != null);
		using var stream = _clientAssembly!.GetManifestResourceStream($"{_clientName}.Resources.{path}");

		Log.Assert(stream != null, $"Failed to load client resource of name '{path}'!");

		var data = new byte[stream.Length];
		stream.ReadExactly(data, 0, data.Length);

		var res = new T();
		res.Load(data);

		return res;
	}

	internal static void Initialize()
	{
		_engineAssembly = typeof(Resource).Assembly;

		var files = Directory.EnumerateFiles(".");
		string exePath = files.First(x => x.EndsWith(".exe"));
		string fileNoExt = exePath[..exePath.LastIndexOf('.')];

		_clientName = fileNoExt[(fileNoExt.LastIndexOf('\\') + 1)..];
		_clientAssembly = Assembly.LoadFrom($"{fileNoExt}.dll");

		// HACK: Referencing DLL directly by file means we can merge DLL into EXE, which means we have many more exposed DLLs.
	}

	/// <summary>
	/// Gets an engine embedded resource as a stream.
	/// <br>The path is relative to Engine/Resources.</br>
	/// </summary>
	/// <returns>A stream of the returned resource or null if it wasn't found.
	/// <br>Remeber to dispose of the stream.</br></returns>
	internal static Stream LoadFromEngine(string path)
	{
		path = FormatPath(path);

		path = $"Engine.Resources.{path}";
		var stream = _engineAssembly!.GetManifestResourceStream(path);

		Log.Assert(stream != null, $"Failed to load engine resource '{path}'!");

		return stream;
	}

	private static string FormatPath(string path)
	{
		path = path.Replace(' ', '_');
		path = path.Replace('/', '\\');
		path = path.Replace('\\', '.');

		return path;
	}
}
