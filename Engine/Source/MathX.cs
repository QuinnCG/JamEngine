using OpenTK.Mathematics;
using System.Security.Claims;
using System;

namespace Engine;

// TODO: Add smooth dampening.

/// <summary>
/// A math class that supports floats, integers, and vectors.
/// </summary>
public static class MathX
{
	public const float PI = 3.141592653589f;

	public static float Sin(float x)
	{
		return MathF.Sin(x);
	}
	public static float Cos(float x)
	{
		return MathF.Cos(x);
	}
	public static float Tan(float x)
	{
		return MathF.Tan(x);
	}

	public static float Atan(float x)
	{
		return MathF.Atan(x);
	}

	public static float Atan2(float x, float y)
	{
		return MathF.Atan2(y, x);
	}

	public static float Lerp(float x, float y, float t)
	{
		return (x * t) + (y * (1f - t));
	}

	public static float Clamp(float x, float min, float max)
	{
		return MathF.Min(max, MathF.Max(min, x));
	}

	public static float InverseLerp(float x, float y, float t)
	{
		return (t - x) / (y - x);
	}

	public static float Range(float x, float y, float min, float max, float t)
	{
		return Lerp(min, max, InverseLerp(x, y, t));
	}

	public static float Max(float x, float y)
	{
		return y > x ? y : x;
	}

	public static float Min(float x, float y)
	{
		return y < x ? y : x;
	}

	/* EXTENSIONS */

	/// <summary>
	/// Contructs and returns a <c>Vector3</c> with the same <c>X</c> and <c>Y</c> values as the <c>Vector2</c> provided.
	/// </summary>
	/// <param name="z">The value of the <c>Z</c> component of the returned <c>Vector3</c>.
	/// <br><c>0</c> by default.</br></param>
	/// <returns></returns>
	public static Vector3 ToVector3(this Vector2 v, float z = 0f)
	{
		return new Vector3()
		{
			X = v.X,
			Y = v.Y,
			Z = z
		};
	}

	public static Vector2 WithX(this Vector2 v, float x)
	{
		return new(x, v.Y);
	}
	public static Vector2 WithY(this Vector2 v, float y)
	{
		return new(v.X, y);
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

	/// <summary>
	/// Converts radians to degrees.
	/// </summary>
	public static float ToDegrees(this float radians)
	{
		return 180f / MathX.PI * radians;
	}

	/// <summary>
	/// Converts degrees to radians.
	/// </summary>
	public static float ToRadians(this float degrees)
	{
		return MathX.PI / 180f * degrees;
	}

	public static Vector2 NormalizedOrZero(this Vector2 v)
	{
		if (v.LengthSquared == 0f)
			return Vector2.Zero;

		return v.Normalized();
	}
}
