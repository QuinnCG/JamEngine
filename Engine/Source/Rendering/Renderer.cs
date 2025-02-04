using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

public static class Renderer
{
	// TODO: Implement framerate. Literal framerate but also an averaged one that uses velocity to slowly update to actual framework to avoid rapid jitter.
	public static int Framerate { get; private set; }
	public static Color4 ClearColor { get; set; } = Color4.Black;

	private static HashSet<RenderHook> _hooks = [];

	public static void RegisterHook(RenderHook hook)
	{
		_hooks.Add(hook);
		SortHooks();

		hook.OnLayerChange += SortHooks;
	}

	public static void UnregisterHook(RenderHook hook)
	{
		_hooks.Remove(hook);
		SortHooks();

		hook.OnLayerChange -= SortHooks;
	}

	internal static void Initialize()
	{
		SpriteRenderer.Initialize();
	}

	internal static void Render()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		foreach (var hook in _hooks)
		{
			uint indices = hook.Bind();
			
			// Skip indicies if value is <= 0.
			if (indices > 0)
			{
				GL.DrawElements(PrimitiveType.Triangles, (int)indices, DrawElementsType.UnsignedInt, 0);
			}
		}
	}

	internal static void CleanUp()
	{
		SpriteRenderer.CleanUp();
	}

	private static void SortHooks()
	{
		_hooks = [.. _hooks.OrderBy(h => h.Layer())];
	}
}
