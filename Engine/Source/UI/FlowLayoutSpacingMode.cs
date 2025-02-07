namespace Engine.UI;

public enum FlowLayoutSpacingMode
{
	/// <summary>
	/// Spacing is applied only between two child entities; not between a child entity and the side of the parent rect.
	/// </summary>
	/// <remarks>[X##Y##Z]</remarks>
	BetweenOnly,
	/// <summary>
	/// Spacing is applied, not just between child entities, but also on the sides of the rect.
	/// </summary>
	/// <remarks>[##X##Y##Z##]</remarks>
	AllEqual,
	/// <summary>
	/// Spacing is applied, not just between child entities, but also on the sides of the rect; but the side spacing is halved.
	/// </summary>
	/// <remarks>[#X##Y##Z#]</remarks>
	SidesHalf
}
