namespace Engine;

public abstract class Resource
{
	private static readonly Dictionary<string, Resource> _resPathToData = [];
	private static readonly Dictionary<string, int> _resPathToRefCount = [];

	public static int GetReferenceCount(string path)
	{
		if (_resPathToRefCount.TryGetValue(path, out int count))
		{
			return count;
		}

		return 0;
	}

	public static T Load<T>(string path) where T : Resource, new()
	{
		IncrementRefCount(path);

		if (_resPathToData.TryGetValue(path, out Resource? resource))
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
			// TODO: Load resources from file. Create handle.
		}

		Log.Error($"Failed to find resource with path '{path}'!");
		return null;
	}

	public static void Release(string path)
	{
		bool freeData = DecrementRefCount(path);
		
		if (freeData)
		{
			_resPathToData[path].OnFree();
			_resPathToData.Remove(path);
		}
	}

	protected abstract void OnLoad();
	protected abstract void OnRelease();
	protected abstract void OnFree();

	private static void IncrementRefCount(string path)
	{
		if (!_resPathToRefCount.TryAdd(path, 1))
		{
			_resPathToRefCount[path] += 1;
		}
	}

	private static bool DecrementRefCount(string path)
	{
		if (_resPathToRefCount.ContainsKey(path))
		{
			_resPathToRefCount[path] -= 1;

			if (_resPathToRefCount.Count <= 0)
			{
				_resPathToRefCount.Remove(path);
				return true;
			}
		}

		return false;
	}
}
