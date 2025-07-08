using OpenTK.Mathematics;

namespace Engine;

/// <summary>
/// A static utility class for easily generating various randon values.
/// </summary>
public static class Rand
{
	/// <summary>
	/// Get a random float at least 0f and less than 1f.<br/>
	/// Each time you read from this property, a new random number is generated and returned.
	/// </summary>
	public static float Value => _shared.NextSingle();

	// The internal shared System.Random instance.
	private static readonly Random _shared = new();

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

	public static float Next(float max)
	{
		return Value * max;
	}
	public static float Next(float min, float max)
	{
		float size = max - min;
		return min + (Value * size);
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

	public static Color4 Color(float minH, float maxH, float minS = 1f, float maxS = 1f, float minV = 1f, float maxV = 1f, float alpha = 1f)
	{
		return Color4.FromHsv(new(Next(minH, maxH), Next(minS, maxS), Next(minV, maxV), alpha));
	}
}
