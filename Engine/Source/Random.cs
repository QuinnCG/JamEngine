namespace Engine;

public static class Random
{
	public static float Value => _shared.NextSingle();

	private static readonly System.Random _shared = new();

	public static int Next()
	{
		return _shared.Next();
	}
	public static int Next(int maxExclusive)
	{
		return _shared.Next(maxExclusive);
	}
	public static int Next(int minInclusive, int maxExclusive)
	{
		return _shared.Next(minInclusive, maxExclusive);
	}

	/// <summary>
	/// Shuffles the given array. This won't create and return a copy, but rather modify the original array.
	/// </summary>
	public static void Shuffle<T>(T[] array)
	{
		_shared.Shuffle(array);
	}

	/// <summary>
	/// Create and return an array of items that were randomly selected from the given pool.<br/>
	/// The length of the final array will be fixed.
	/// </summary>
	/// <param name="pool">The pool of items that may possibly be added to the final array.</param>
	/// <param name="length">The length of the final array.</param>
	/// <returns>The final array with random, possibly duplicate, items from the pool.</returns>
	public static T[] GetItems<T>(T[] pool, int length)
	{
		return _shared.GetItems(pool, length);
	}
}
