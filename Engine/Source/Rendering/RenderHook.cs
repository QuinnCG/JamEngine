namespace Engine.Rendering;

/// <summary>
/// A handle for a renderable object.
/// </summary>
public class RenderHook(Func<int> bind, Func<RenderLayer> layer) : IComparable
{
	/// <summary>
	/// When this is called, bind anything you wish to be rendered.<br/>
	/// The return value should either be the number of indices to draw or 0 to denote nothing should be rendered; this can be used for disabled entities.
	/// </summary>
	internal Func<int> Bind = bind;
	/// <summary>
	/// The render layer for this <see cref="RenderHook"/>.
	/// </summary>
	internal Func<RenderLayer> Layer = layer;

	internal event Action? OnLayerChange;

	/// <summary>
	/// Call this when changing the return value of <see cref="Layer"/>.<br/>
	/// Doing so will trigger a sorting of all currently registered <see cref="RenderHook"/>s.
	/// </summary>
	public void NotifyLayerChange()
	{
		OnLayerChange?.Invoke();
	}

	public int CompareTo(object? obj)
	{
		return Layer().Order.CompareTo(obj);
	}
}
