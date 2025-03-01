namespace Engine;

// TODO: [Resource.cs] Implement resource system.
// It should support thing like: text files, bin files, GPU textures, GPU shaders, World (which is just a bunch of serialized entities).

// https://github.com/QuinnCG/JamEngine/issues/5
// Issue Summary:

// How are resources to be handled?
// When editing a world, you want to be able to save it, but if that world is stored in a Resources folder next to the Source code folder, then the built version won't have access to this?
// One possibility would be to copy resources across in development or release builds, but not editor builds.Then in editor builds, you reference the source code resources folder.
// If this approach is decided upon, there should be an abstraction on getting the raw file data for a resource, so that development and release builds (mainly release builds) can pack the resource data however they like.

public abstract class Resource
{
	/// <summary>
	/// Load a resource from disk.
	/// </summary>
	/// 
	/// <remarks>
	/// In <see cref="BuildType.Editor"/>, resources are loaded directly from the <c>Resource</c> folder located next to the <c>Source</c> code folder<br/>
	/// For any other <see cref="BuildType"/>, resources are loaded from a different location.
	/// </remarks>
	/// 
	/// <typeparam name="T"></typeparam>
	/// <param name="path">This doesn't have to be the file path, just the path to the resource, which may or may not relate.</param>
	/// <returns>A new instance of the resource or an existing reference if the path was already loaded.</returns>
	public static T Load<T>(string path) where T : Resource
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Load a resource that is embedded in the engine's DLL.
	/// </summary>
	/// <returns>A new instance of the resource or an existing reference if the path was already loaded.</returns>
	public static T LoadEngineResource<T>(string path) where T : Resource
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Releases any resource (engine embedded or otherwise).<br/>
	/// Releasing means to decrement the reference count and only if there are no more references will <see cref="OnFree"/> be called.
	/// </summary>
	/// <param name="path">The path used to load the specified resource.</param>
	public static void ReleaseResource(string path)
	{
		throw new NotImplementedException();
	}

	// No reference counting. Just plain load the reference.
	// Do account for whether we are in an Editor build or not.
	public static byte[] LoadResourceRaw(string path)
	{
		foreach (var file in GetResourcesDirectory().GetFiles())
		{
			Log.Info(file.FullName);
		}

		throw new NotImplementedException();
	}

	private static DirectoryInfo GetResourcesDirectory()
	{
		var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

		while (directory != null && directory.GetFiles("*.sln").Length == 0)
		{
			directory = directory.Parent;
		}

		if (directory == null)
		{
			throw new Exception("Could not find the solution directory.");
		}

		foreach (var dir in directory.GetDirectories())
		{
			if (dir.Name != "Engine")
			{
				directory = dir.GetDirectories("Resources").FirstOrDefault();

				if (directory != null)
				{
					break;
				}
			}
		}

		if (directory == null)
		{
			throw new Exception("Could not find the resource directory. It should be named 'Resources'.");
		}

		return directory;
	}

	protected abstract void OnLoad();
	protected abstract void OnFree();
}
