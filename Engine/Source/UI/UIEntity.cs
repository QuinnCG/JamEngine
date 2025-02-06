using Engine.Rendering;

namespace Engine.UI;

public class UIEntity : Entity
{
	// Supports a UI transform.
	// TODO: How do we handle the top-most level? A special canvas entity?

	// TODO: Need concept of being focused, such as mouse is hovering over and not being covered; does this require a physics raycast or can we fake it?
	// Mayne no concept of focus, just interface callbacks for mouse events such as enter and exit. To check for events, check if mouse is within any rects for UI in front of target.

	// Top down priority made bottom up? e.g. children ask parent for calculated transform data.
	// Canvas is a type of UI Entity that works like all others except it forces it size to be that of the screen relative to its setting.
	// UI entities are flexboxes first and foremost. A special type holds absolute positional data and overrides the request for transform from its parent.
	// Rendering is handled as an override but handled by each entity.
	// Basic UI entity has no visuals, instead there's an image entity for that.

	public UIRect Rect
	{
		get
		{
			if (Parent is UIEntity parent)
			{
				return parent.CalculateRect(this);
			}
			else if (Parent is UICanvas canvas)
			{
				return canvas.CalculateRect(this);
			}

			Log.Error($"UI entity '{Name}' isn't the child of another UI entity or a UI canvas. It must be of one!");
			return default;
		}
	}
	public UICanvas Canvas => _canvas!;
	public RenderLayer Layer { get; set; } = RenderLayer.Default;

	private UICanvas? _canvas;

	public void SetCanvas(UICanvas canvas)
	{
		_canvas = canvas;

		foreach (var child in Children)
		{
			if (child is UIEntity ui)
			{
				ui.SetCanvas(canvas);
			}
		}
	}

	protected virtual UIRect CalculateRect(UIEntity child)
	{
		throw new NotImplementedException();
	}
}
