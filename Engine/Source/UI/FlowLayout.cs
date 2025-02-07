namespace Engine.UI;

// TODO: Implement.
public class FlowLayout : UIEntity
{
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
	private float _spacing;
	private FlowLayoutSpacingMode _spacingMode;

	protected override UIRect CalculateRect(UIEntity child)
	{
		return base.CalculateRect(child);
	}
}
