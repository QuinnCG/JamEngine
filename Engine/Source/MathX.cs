using OpenTK.Mathematics;

namespace Engine;

public static class MathX
{
	public const float PI = 3.141592653589f;

	public const float RadToDeg = 180f / PI;
	public const float DegToRad = PI / 180f;

	/// <summary>
	/// If the vector is zero, return <see cref="Vector2.Zero"/> instead of a <see cref="Vector2"/> of <see cref="float.NaN"/>.
	/// </summary>
	public static Vector2 NormalizedOrZero(this Vector2 v)
	{
		if (v.LengthSquared == 0f)
		{
			return Vector2.Zero;
		}

		return v.Normalized();
	}

	public static Vector2 WithX(this Vector2 v, float x)
	{
		return new Vector2(x, v.Y);
	}
	public static Vector2 WithY(this Vector2 v, float y)
	{
		return new Vector2(v.X, y);
	}

	public static Vector2 BoundsLeft(this Vector2 v)
	{
		return v.X / 2f * -Vector2.UnitX;
	}
	public static Vector2 BoundsRight(this Vector2 v)
	{
		return v.X / 2f * Vector2.UnitX;
	}
	public static Vector2 BoundsTop(this Vector2 v)
	{
		return v.Y / 2f * Vector2.UnitY;
	}
	public static Vector2 BoundsBottom(this Vector2 v)
	{
		return v.Y / 2f * -Vector2.UnitY;
	}

	/// <summary>
	/// Converts radians to degrees.
	/// </summary>
	/// <returns>Degrees.</returns>
	public static float ToDegrees(this float radians)
	{
		return radians * RadToDeg;
	}
	/// <summary>
	/// Converts degrees to radians.
	/// </summary>
	/// <returns>Radians.</returns>
	public static float ToRadians(this float degrees)
	{
		return degrees * DegToRad;
	}

	public static float Clamp(float value, float min, float max)
	{
		return Math.Clamp(value, min, max);
	}
	public static float Max(float x, float y)
	{
		return MathF.Max(x, y);
	}
	public static float Min(float x, float y)
	{
		return MathF.Min(x, y);
	}

	// TODO: [MathX.cs] Max and Min functions with params float[] values.

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
	public static float Atan2(float y, float x)
	{
		return MathF.Atan2(y, x);
	}

	public static float Lerp(float a, float b, float t)
	{
		return a + (b - a) * t;
	}
}
