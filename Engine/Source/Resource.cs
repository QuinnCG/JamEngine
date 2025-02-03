namespace Engine;

// TODO: Remake this?

public abstract class Resource
{
	private static readonly Dictionary<string, object> _resPathToData = [];
	private static readonly Dictionary<string, int> _resPathToRefCount = [];

	// Ref count loaded resources and only delete the data when last ref is unreferenced.

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

		if (_resPathToData.TryGetValue(path, out var data))
		{
			var resource = new T();
			resource.OnLoad(data);

			return resource;
		}
		else
		{
			// TODO: Load data from file, increment ref count, return resource instance.
			throw new NotImplementedException();

			//_resPathToData.Add(path, null);
		}
	}

	public static void Release(string path)
	{
		bool freeData = DecrementRefCount(path);
		
		if (freeData)
		{
			_resPathToData.Remove(path);
		}
	}

	protected abstract void OnLoad(object data);
	protected abstract void OnRelease();

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
