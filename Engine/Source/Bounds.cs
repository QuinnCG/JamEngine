using OpenTK.Mathematics;

namespace Engine;

/// <summary>
/// Describes an arbitrary bounds.
/// </summary>
public struct Bounds(Vector2 center, Vector2 size)
{
	public Vector2 Center = center;
	public Vector2 Size = size;

	public readonly Vector2 Left => Center - new Vector2(Size.X / 2f, 0f);
	public readonly Vector2 Right => Center + new Vector2(Size.X / 2f, 0f);
	public readonly Vector2 Top => Center + new Vector2(0f, Size.Y / 2f);
	public readonly Vector2 Bottom => Center - new Vector2(0f, Size.Y / 2f);

	public readonly Vector2 BottomLeft => new(Left.X, Bottom.Y);
	public readonly Vector2 BottomRight => new(Right.X, Bottom.Y);
	public readonly Vector2 TopLeft => new(Left.X, Top.Y);
	public readonly Vector2 TopRight => new(Right.X, Top.Y);

	public readonly bool IsPointInside(Vector2 point)
	{
		if (point.X > Right.X) return false;
		if (point.X < Left.X) return false;
		if (point.Y > Top.Y) return false;
		if (point.Y < Bottom.Y) return false;

		return true;
	}

	public readonly bool IsPointOutside(Vector2 point)
	{
		return !IsPointInside(point);
	}

	public readonly bool IsTouchingBounds(Bounds bounds)
	{
		if (bounds.Left.X > Right.X) return false;
		if (bounds.Right.X < Left.X) return false;
		if (bounds.Bottom.Y > Top.Y) return false;
		if (bounds.Top.Y < Bottom.Y) return false;

		return true;
	}
	public readonly bool IsNotTouchingBounds(Bounds bounds)
	{
		return !IsTouchingBounds(bounds);
	}
}
