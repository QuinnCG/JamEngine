using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

public static class Renderer
{
	public static Color4 ClearColor { get; set; } = Color4.Black;

	private static readonly List<IRenderable> _renderables = [];

	public static void Register(IRenderable renderable)
	{
		_renderables.Add(renderable);
	}

	public static void Unregister(IRenderable renderable)
	{
		_renderables.Remove(renderable);
	}

	internal static void Render()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		Sort();

		foreach (var renderable in _renderables)
		{
			int indexCount = renderable.Render();
			
			if (indexCount > 0)
			{
				GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
			}
		}
	}

	private static void Sort()
	{
		var ordered = _renderables.OrderBy(layer => layer.GetRenderLayer());
		_renderables.Clear();
		_renderables.AddRange(ordered);
	}
}
