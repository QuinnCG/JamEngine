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
	public UIRect Rect
	{
		get => _rect;
		set
		{
			_rect = value;
			
			if (IsCreated)
			{
				Canvas.RegenerateLayout();
			}
		}
	}
	/// <summary>
	/// If true, this <see cref="UIEntity"/>'s rect will be made to fit it's content tightly.<br/>
	/// It's important to note that the child UI entities must rects will be left as is, if this setting is enabled.
	/// </summary>
	public bool DoesWrapContent { get; set; }
	/// <summary>
	/// Whether to use <see cref="Rect"/> as an absolute position or to ignore it and leave this entity's layout up to its parent.
	/// </summary>
	public bool DoesFlex { get; set; } = true;

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

	public IEnumerable<UIEntity> UIChildren => _uiChildren;
	public int UIChildrenCount => _uiChildren.Count;

	/// <summary>
	/// The <see cref="Rendering.RenderHook"/> for this UI entity.<br/>
	/// By default, it is not registered. If you wish for this entity's <see cref="OnRender"/> method to be called, you first call <see cref="Renderer.RegisterHook(RenderHook)"/>.<br/>
	/// <see cref="Renderer.UnregisterHook(RenderHook)"/> will be called for you automatically, upon this entity's destruction.
	/// </summary>
	protected RenderHook RenderHook => _renderHook!;
	private RenderHook? _renderHook;

	private readonly HashSet<UIEntity> _uiChildren = [];

	private UICanvas? _canvas;
	private UIRect _rect = new(Vector2.Zero, Vector2.One);

	private Vector2 _renderPos;
	private float _renderRot;
	private Vector2 _renderScale = Vector2.One;

	// Cache rect and update on global canvas update. Maybe any ui can tell canvas to update its entities then it calls an update event that they all subbed to.
	// Also, ad render transform (pos, rot, scale) that is applied on top of UI rect and inherits from parent render transform.

	protected override void OnCreate()
	{
		_renderHook = new RenderHook(OnRender, () => Canvas.Layer);
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
	}

	public void RegenerateLayout()
	{
		OnRegenerateLayout();

		foreach (var child in UIChildren)
		{
			child.RegenerateLayout();
		}
	}

	/// <summary>
	/// Primarily for use by <see cref="UICanvas"/>.<br/>
	/// This avoids triggering a canvas level regeneration, which will just ripple down and retrigger the UI entities, making an infinite loop.
	/// </summary>
	internal void SetRect_Internal(UIRect rect)
	{
		_rect = rect;
	}

	protected virtual void OnRegenerateLayout()
	{
		foreach (var child in UIChildren)
		{
			child.SetRect_Internal(Rect);
		}
	}

	protected virtual int OnRender() => 0;

	protected override void OnDestroy()
	{
		Log.Assert(_canvas != null, $"UI entity '{Name}' doesn't have a set canvas! Are they a direct/indirect child of a canvas entity?");
		Renderer.UnregisterHook(_renderHook!);
	}

	protected override void OnChildAdded(Entity child)
	{
		if (child is UIEntity ui)
		{
			_uiChildren.Add(ui);
		}
	}

	protected override void OnChildRemoved(Entity child)
	{
		if (child is UIEntity ui)
		{
			_uiChildren.Remove(ui);
		}
	}
}
