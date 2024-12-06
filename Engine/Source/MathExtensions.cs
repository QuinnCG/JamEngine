using OpenTK.Mathematics;

namespace Engine;

public static class MathExtensions
{
	public static Vector3 ToVector3(this Vector2 v, float z = 0f)
	{
		return new Vector3()
		{
			X = v.X,
			Y = v.Y,
			Z = z
		};
	}

	public static float Distance(this Vector2 v1, Vector2 v2)
	{
		return Vector2.Distance(v1, v2);
	}
	public static float DistanceSquared(this Vector2 v1, Vector2 v2)
	{
		return Vector2.DistanceSquared(v1, v2);
	}

	public static Vector2 DirectionTo(this Vector2 start, Vector2 end)
	{
		return (end - start).Normalized();
	}
}
