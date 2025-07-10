using System.Text;

namespace Engine;

public abstract class Resource
{
	private static readonly Dictionary<string, (Resource res, int refCount)> _loadedResources = [];

	/// <summary>
	/// The path used for the resource.<br/>
	/// This is not the full OS path, but one that starts just below the <c>Resources</c> folder.
	/// </summary>
	public string RelativePath { get; private set; } = string.Empty;
	/// <summary>
	/// The full OS path.
	/// </summary>
	public string FullPath { get; private set; } = string.Empty;
	public bool IsEngineResource { get; private set; }

	/// <summary>
	/// Writes all active resources to the console.
	/// </summary>
	public static void LogResources()
	{
		var builder = new StringBuilder();

		builder.AppendLine("Resources:");

		foreach (var pair in _loadedResources.Values)
		{
			var res = pair.res;
			builder.AppendLine($"  - {res.GetType().Name}");
		}

		if (_loadedResources.Count == 0)
		{
			builder.AppendLine("  - No Resources Loaded");
		}

		Log.Info(builder.ToString());
	}

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
	public static T Load<T>(string path) where T : Resource, new()
	{
		if (_loadedResources.TryGetValue(path, out (Resource, int) existingPair))
		{
			var (existingRes, existingRefCount) = existingPair;

			// Increment ref count.
			_loadedResources[path] = (existingRes, existingRefCount + 1);

			// Return existing resource.
			return existingRes as T;
		}

		byte[] data = LoadResourceRaw(path);

		var res = new T()
		{
			IsEngineResource = false,
			RelativePath = path,
			FullPath = GetResourcesDirectory().FullName + "\\" + path
		};
		res.OnLoad(data);

		_loadedResources.Add(path, (res, 1));

		return res;
	}

	/// <summary>
	/// Load a resource that is embedded in the engine's DLL.
	/// </summary>
	/// <returns>A new instance of the resource or an existing reference if the path was already loaded.</returns>
	public static T LoadEngineResource<T>(string path) where T : Resource, new()
	{
		path = path.Replace('/', '\\').Replace('\\', '.');
		path = $"Engine.Resources.{path}";

		if (_loadedResources.TryGetValue(path, out (Resource, int) existingPair))
		{
			var (existingRes, existingRefCount) = existingPair;

			// Increment ref count.
			_loadedResources[path] = (existingRes, existingRefCount + 1);

			// Return existing resource.
			return existingRes as T;
		}

		var stream = typeof(Resource).Assembly.GetManifestResourceStream(path)
			?? throw new Exception($"Failed to get engine resource '{path}'!");

		var data = new byte[stream.Length];
		stream.ReadExactly(data, 0, data.Length);

		var res = new T()
		{
			IsEngineResource = true,
			RelativePath = path,
			FullPath = path
		};

		_loadedResources.Add(path, (res, 1));

		res.OnLoad(data);

		return res;
	}

	/// <summary>
	/// Releases any resource (engine embedded or otherwise).<br/>
	/// Releasing means to decrement the reference count and only if there are no more references will <see cref="OnFree"/> be called.
	/// </summary>
	/// <param name="path">The path used to load the specified resource.</param>
	public static void Release(string path)
	{
		if (_loadedResources.TryGetValue(path, out (Resource res, int refCount) value))
		{
			var (res, refCount) = value;

			// Decrement ref count.
			_loadedResources[path] = (res, refCount - 1);

			// Free resource if ref count is <= 0.
			if (_loadedResources[path].refCount <= 0)
			{
				_loadedResources[path].res.OnFree();
				_loadedResources.Remove(path);
			}
		}
	}

	// No reference counting. Just plain load the reference.
	// Do account for whether we are in an Editor build or not.
	private static byte[] LoadResourceRaw(string path)
	{
		var files = GetResourcesDirectory().GetFiles(path, SearchOption.AllDirectories);

		if (files.Length == 0)
		{
			return [];
		}

		return File.ReadAllBytes(files[0].FullName);
	}

	// TODO: [Resource.cs] GetResourcesDirectory should be cached.

	/// <inheritdoc cref="Release(string)"/>
	public void Release()
	{
		Release(FullPath);
	}

	private static DirectoryInfo GetResourcesDirectory()
	{
#if EDITOR
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
#else
		string path = Directory.GetCurrentDirectory() + "\\Resources";
		return new DirectoryInfo(path);
#endif
	}

	protected abstract void OnLoad(byte[] data);
	protected abstract void OnFree();
}
