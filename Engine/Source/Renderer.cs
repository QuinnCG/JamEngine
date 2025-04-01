using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine;

public static class Renderer
{
	public static Color4 ClearColor { get; set; } = Color4.Black;

	internal static void Render()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		World.RenderWorlds();
	}
}
