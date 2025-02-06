namespace Engine.UI;

public class UICanvas : SpatialEntity
{
	public CanvasMode Mode { get; set; } = CanvasMode.ScreenSpace;

	protected override void OnCreate()
	{
		foreach (var child in Children)
		{
			if (child is UIEntity ui)
			{
				ui.SetCanvas(this);
			}
		}
	}

	public UIRect CalculateRect(UIEntity child)
	{
		// if mode == WorldSpace

		throw new NotImplementedException();
	}
}
