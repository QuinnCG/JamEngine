using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

// TODO: [IRenderable.cs] Implement system that uses this.
// When an entity is added to a world, that world should get all interfaces from the entity of IRenderable.
// Create an API for getting all interfaces on an entity of a type; e.g. from itself, components, and children.

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
