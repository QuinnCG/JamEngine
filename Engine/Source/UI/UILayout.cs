using OpenTK.Mathematics;

namespace Engine.UI;

public class UILayout : UIEntity
{
	// TODO: Support spacing.
	// TODO: Support vertical mode and reverse option.

	protected override Vector2 GetFlexPositon(Entity child)
	{
		int i = GetChildIndex(child);
		int count = ChildCount;

		float w = (float)i / count * Scale.X;

		float offset = w * i;
		offset += GetFlexScale(child).X / 2f;

		return GetLeft() + (offset * Vector2.UnitX);
	}

	protected override Vector2 GetFlexScale(Entity child)
	{
		return (Scale / ChildCount).WithY(Scale.Y);
	}
}
