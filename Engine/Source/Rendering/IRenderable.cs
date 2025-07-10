using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public interface IRenderable
{
	/// <summary>
	/// The bounds that this render object can be seen within.
	/// </summary>
	public Bounds RenderBounds { get; }

	/// <summary>
	/// Get the instance of the render layer that's used for this object in sorting.
	/// </summary>
	/// <returns>The exact instance of the layer to use for sorting.</returns>
	public RenderLayer GetRenderLayer();
	/// <summary>
	/// Called just before <see cref="GL.DrawElements(PrimitiveType, int, DrawElementsType, int)"/> is called for this <see cref="IRenderable"/>.<br/>
	/// Bind any relavent data, needed for the draw call.
	/// </summary>
	/// <returns>The number of indices to draw. A value of 0 or less will result in this <see cref="IRenderable"/> being skipped.</returns>
	public int Render();
}
