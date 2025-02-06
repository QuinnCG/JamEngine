namespace Engine.UI;

public class UICanvas : SpatialEntity
{
	public CanvasMode Mode { get; set; } = CanvasMode.ScreenSpace;
	public event Action? OnRegenerateLayout;

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

	/// <summary>
	/// Calls <see cref="OnRegenerateLayout"/> which will trigger each <see cref="UIEntity"/> to recalculate its layout.
	/// </summary>
	public void RegenerateLayout()
	{
		OnRegenerateLayout?.Invoke();
	}
}
