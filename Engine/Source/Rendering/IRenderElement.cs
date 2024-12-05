namespace Engine.Rendering;

/// <summary>
/// This is not something to be rendered. This is any bindable and dispoable item used during rendering.
/// </summary>
internal interface IRenderElement : IDisposable
{
	public void Bind();
}
