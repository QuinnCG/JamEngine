namespace Engine;

public static class Resources
{
	/// <summary>
	/// Gets an engine embedded resource as a stream.
	/// <br>The path is relative to Engine/Resources.</br>
	/// </summary>
	/// <returns>A stream of the returned resource or null if it wasn't found.
	/// <br>Remeber to dispose of the stream.</br></returns>
	internal static Stream? GetEngineResource(string path)
	{
		path = $"Engine.Resources.{path}";
		return typeof(Resources).Assembly.GetManifestResourceStream(path);
	}
}
