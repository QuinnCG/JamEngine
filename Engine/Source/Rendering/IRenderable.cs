using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

// TODO: [IRenderable.cs] Implement system that uses this.

public interface IRenderable
{
	/// <summary>
	/// Called when this element is to be rendered.
	/// </summary>
	/// <returns>How many indices to pass to the <see cref="GL.DrawElements(BeginMode, int, DrawElementsType, int)"/> method.</returns>
	public int Bind();
	/// <summary>
	/// The render layer used for sorting this element.
	/// </summary>
	/// <returns></returns>
	public RenderLayer GetRenderLayer();
}
