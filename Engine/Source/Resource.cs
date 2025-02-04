using System.Reflection;

namespace Engine;

public abstract class Resource
{
	private static readonly Dictionary<string, Resource> _pathToRes = [];
	private static readonly Dictionary<string, int> _pathToRefCount = [];

	private string _path = string.Empty;

	public static int GetReferenceCount(string path)
	{
		if (_pathToRefCount.TryGetValue(path, out int count))
		{
			return Math.Max(0, count);
		}

		return 0;
	}

	public static T Load<T>(string path) where T : Resource, new()
	{
		IncrementRefCount(path);

		if (_pathToRes.TryGetValue(path, out Resource? resource))
		{
			if (resource is T t)
			{
				return t;
			}
			else
			{
				Log.Error($"Failed to convert resource at path '{path}' into a resource of type '{typeof(T)}'!");
				return null;
			}
		}
		else
		{
			var newRes = new T()
			{
				_path = path
			};
			_pathToRes.Add(path, newRes);

			string resourcesPath = Assembly.GetEntryAssembly()!.Location;
			resourcesPath = resourcesPath[..resourcesPath.LastIndexOf('\\')];

			string resPath = resourcesPath + "/Resources" + $"/{path}";

			if (!File.Exists(resPath))
			{
				Log.Error($"Failed to find resource with path '{path}'!");
				return null;
			}

			byte[] data = File.ReadAllBytes(resPath);
			newRes.OnLoad(data);

			return newRes;
		}
	}

	public static void Release(string path)
	{
		bool freeData = DecrementRefCount(path);
		
		if (freeData)
		{
			_pathToRes[path].OnFree();
			_pathToRes.Remove(path);
		}
	}

	/// <summary>
	/// You must still call Release.
	/// </summary>
	internal static T LoadEngineResource<T>(string path) where T : Resource, new()
	{
		path = path.Replace('/', '\\');
		path = path.Replace('\\', '.');

		path = $"Engine.Resources.{path}";

		Assembly asm = Assembly.GetAssembly(typeof(Resource))!;
		Stream resStream = asm.GetManifestResourceStream(path)!;

		var memStream = new MemoryStream();
		resStream.CopyTo(memStream);

		var res = new T()
		{
			_path = path
		};
		res.OnLoad(memStream.ToArray());

		IncrementRefCount(path);
		return res;
	}

	public void Release()
	{
		Release(_path);
	}

	/// <summary>
	/// Called only if the resource at the specified path isn't already loaded.<br/>
	/// Otherwise, it will use the existing resource instance associated with the specified path.
	/// </summary>
	/// <param name="data">The raw data in bytes of the underlying resource.</param>
	protected abstract void OnLoad(byte[] data);
	/// <summary>
	/// Called when the final reference count is released for a given path.
	/// </summary>
	protected abstract void OnFree();

	private static void IncrementRefCount(string path)
	{
		if (!_pathToRefCount.TryAdd(path, 1))
		{
			_pathToRefCount[path] += 1;
		}
	}

	private static bool DecrementRefCount(string path)
	{
		if (_pathToRefCount.ContainsKey(path))
		{
			_pathToRefCount[path] -= 1;

			if (_pathToRefCount.Count <= 0)
			{
				_pathToRefCount.Remove(path);
				return true;
			}
		}

		return false;
	}
}
