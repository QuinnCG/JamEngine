using Engine.Rendering;
using OpenTK.Mathematics;

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

	public UICanvas Canvas => _canvas!;
	public RenderLayer Layer { get; set; } = RenderLayer.Default;
	public UIRect Rect => _rect;

	public Vector2 RenderPosition
	{
		get
		{
			if (Parent is UIEntity ui)
			{
				return ui.RenderPosition + _renderPos;
			}

			return _renderPos;
		}
		set => _renderPos = value;
	}
	public float RenderRotation
	{
		get
		{
			if (Parent is UIEntity ui)
			{
				return ui.RenderRotation + _renderRot;
			}

			return _renderRot;
		}
		set => _renderRot = value;
	}
	public Vector2 RenderScale
	{
		get
		{
			if (Parent is UIEntity ui)
			{
				return ui.RenderScale + _renderScale;
			}

			return _renderScale;
		}
		set => _renderScale = value;
	}

	private RenderHook? _renderHook;

	private UICanvas? _canvas;
	private UIRect _rect;

	private Vector2 _renderPos;
	private float _renderRot;
	private Vector2 _renderScale = Vector2.One;

	// Cache rect and update on global canvas update. Maybe any ui can tell canvas to update its entities then it calls an update event that they all subbed to.
	// Also, ad render transform (pos, rot, scale) that is applied on top of UI rect and inherits from parent render transform.

	protected override void OnCreate()
	{
		_renderHook = new RenderHook(OnRender, GetRenderLayer);
		Renderer.RegisterHook(_renderHook);
	}

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

		_canvas.OnRegenerateLayout += OnRegenerateLayout;
	}

	protected virtual UIRect CalculateRect(UIEntity child)
	{
		throw new NotImplementedException();
	}

	protected virtual int OnRender() => 0;

	private RenderLayer GetRenderLayer()
	{
		return Layer;
	}

	private void OnRegenerateLayout()
	{
		if (Parent is UIEntity parent)
		{
			_rect = parent.CalculateRect(this);
		}
		else if (Parent is UICanvas canvas)
		{
			_rect = canvas.CalculateRect(this);
		}
		else
		{
			Log.Error($"UI entity '{Name}' isn't the child of another UI entity or a UI canvas. It must be of one!");
		}
	}

	protected override void OnDestroy()
	{
		Log.Assert(_canvas != null, $"UI entity '{Name}' doesn't have a set canvas! Are they a direct/indirect child of a canvas entity?");

		Renderer.UnregisterHook(_renderHook!);
		_canvas.OnRegenerateLayout -= OnRegenerateLayout;
	}
}
