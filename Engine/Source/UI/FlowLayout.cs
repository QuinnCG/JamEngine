using OpenTK.Mathematics;

namespace Engine.UI;

public class FlowLayout : UIEntity
{
	// TODO: Only call regen if value is different from before.

	/// <summary>
	/// The axis for children to flow in. Horizontal or vertical.
	/// </summary>
	/// <remarks>Horizontal by default.</remarks>
	public FlowLayoutDirection Direction
	{
		get => _direction;
		set
		{
			_direction = value;
			Canvas.RegenerateLayout();
		}
	}
	/// <summary>
	/// By default, children will flow left-to-right or up-to-down, depending on what <see cref="Direction"/> is set to.<br/>
	/// You can reverse this to make horizontal layouts go right-to-left, and vertical ones go down-to-up.
	/// </summary>
	public bool IsReversed
	{
		get => _isReversed;
		set
		{
			_isReversed = value;
			Canvas.RegenerateLayout();
		}
	}
	/// <summary>
	/// How the children should be aligned if this UI entity's rect is big enough to have unused space by the children.<br/>
	/// </summary>
	/// <remarks>By default the chosen alignment is <see cref="FlowLayoutAlignment.Left"/>.</remarks>
	public FlowLayoutAlignment Alignment
	{
		get => _alignment;
		set
		{
			_alignment = value;
			Canvas.RegenerateLayout();
		}
	}
	/// <summary>
	/// Alignment for the children on the axis perpendicular to <see cref="Direction"/>.
	/// </summary>
	public FlowLayoutAlignment SecondaryAlignment
	{
		get => _secondaryAlignment;
		set
		{
			_secondaryAlignment = value;
			Canvas.RegenerateLayout();
		}
	}
	/// <summary>
	/// The gaps surrounding the child UI entities.
	/// </summary>
	public float Spacing
	{
		get => _spacing;
		set
		{
			_spacing = value;
			Canvas.RegenerateLayout();
		}
	}
	/// <summary>
	/// By default, <see cref="Spacing"/> will act as padding between child entities.<br/>
	/// You may want spacing, not just between, but also on the outer ends of the children.
	/// </summary>
	/// <remarks>By default <see cref="FlowLayoutSpacingMode.AllEqual"/> is selected.</remarks>
	public FlowLayoutSpacingMode SpacingMode
	{
		get => _spacingMode;
		set
		{
			_spacingMode = value;
			Canvas.RegenerateLayout();
		}
	}

	private FlowLayoutDirection _direction;
	private bool _isReversed;
	private FlowLayoutAlignment _alignment;
	private FlowLayoutAlignment _secondaryAlignment;
	private float _spacing;
	private FlowLayoutSpacingMode _spacingMode;

	protected override UIRect CalculateRect(UIEntity child)
	{
		// Just implement horizontal for now.

		// TODO: UI entity should cache UI children.
		// TODO: Ideally, shouldn't have to recalculate this for every child that asks.
		var uiChildren = Children.Where(x => x is UIEntity).Cast<UIEntity>();
		int childSum = uiChildren.Count();
		float sumLength = uiChildren.Sum(x => x.Rect.Size.X);

		switch (SpacingMode)
		{
			case FlowLayoutSpacingMode.BetweenOnly:
				sumLength += Spacing * (childSum - 1);
				break;
			case FlowLayoutSpacingMode.AllEqual:
				sumLength += Spacing * (childSum + 1);
				break;
			case FlowLayoutSpacingMode.SidesHalf:
				// The last spacing is meant to be the sum of the two halves placed on either end.
				sumLength += (Spacing * (childSum - 1)) + Spacing;
				break;
		}

		Vector2 origin = Rect.Center + Alignment switch
		{
			FlowLayoutAlignment.Left => Rect.Size.BoundsLeft(),
			FlowLayoutAlignment.Center => Vector2.Zero,
			FlowLayoutAlignment.Right => Rect.Size.BoundsRight(),
			_ => throw new Exception()
		};

		int childIndex = uiChildren.Index().First(x => x.Item == child).Index;
		//Vector2 pos = origin + (childIndex * );

		// TODO: Major UI refactor: especially now that rects can be set manually per UI element, we should probably have a top down approach where upon layout regeneration, parents update children.
		// Without this, it becomes difficult for things like flow layout to calculate the offset per child.

		return new UIRect(default, child.Rect.Size);
	}
}
