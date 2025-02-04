using OpenTK.Mathematics;

namespace Engine;

public static class MathX
{
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
}
