namespace Engine;

public static class CollectionExtensions
{
	public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> collection)
	{
		foreach (var item in collection)
		{
			set.Add(item);
		}
	}
	public static void AddRange<T, U>(this Dictionary<T, U> dictionary, IEnumerable<KeyValuePair<T, U>> collection) where T : notnull
	{
		foreach (var item in collection)
		{
			dictionary.Add(item.Key, item.Value);
		}
	}

	public static void RemoveRange<T>(this HashSet<T> set, IEnumerable<T> collection)
	{
		foreach (var item in collection)
		{
			set.Remove(item);
		}
	}
	public static void RemoveRange<T, U>(this Dictionary<T, U> dictionary, IEnumerable<T> collection) where T : notnull
	{
		foreach (var item in collection)
		{
			dictionary.Remove(item);
		}
	}

	/// <summary>
	/// Get a random item from the given collection, or default(T), if the collection is empty.
	/// </summary>
	/// <param name="collection">The collection to take from.</param>
	/// <returns>The randomly chosen item.</returns>
	public static T GetRandom<T>(this ICollection<T> collection)
	{
		if (collection.Count != 0)
		{
			return collection.ElementAt(Rand.Next(0, collection.Count));
		}

		return default;
	}
}
