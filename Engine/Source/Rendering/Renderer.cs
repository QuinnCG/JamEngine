using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine.Rendering;

public static class Renderer
{
	public static Color4 ClearColor { get; set; } = Color4.Black;

	internal static void Initialize()
	{
		GL.LoadBindings(new GLFWBindingsContext());
	}

	internal static void Clear()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);
	}

	internal static void Render()
	{
		// TODO
	}

	internal static void CleanUp()
	{
		// TODO
	}
}
