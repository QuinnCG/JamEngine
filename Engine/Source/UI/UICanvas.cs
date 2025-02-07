using OpenTK.Mathematics;

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
	/// Calculates the model-view-projection matrix, used for rendering, for a given <see cref="UIEntity"/> that's a child of this <see cref="UICanvas"/>.<br/>
	/// Accounts for the <see cref="CanvasMode"/> too.
	/// </summary>
	public Matrix4 CalculateMatrix(UIEntity entity)
	{
		return Matrix4.Identity;
	}

	/// <summary>
	/// Calls <see cref="OnRegenerateLayout"/> which will trigger each <see cref="UIEntity"/> to recalculate its layout.
	/// </summary>
	public void RegenerateLayout()
	{
		OnRegenerateLayout?.Invoke();
	}
}
