using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

public static class Renderer
{
	public static Color4 ClearColor { get; set; } = Color4.Black;

	private static readonly List<IRenderable> Renderables = [];
	private static int LastGLMsgCode = -1;

	public static void Register(IRenderable renderable)
	{
		Renderables.Add(renderable);
	}

	public static void Unregister(IRenderable renderable)
	{
		Renderables.Remove(renderable);
	}

	internal static void Initialize()
	{
		GL.Enable(EnableCap.DebugOutput);
		GL.DebugMessageCallback(OnGLMessage, 0);
	}

	private static unsafe void OnGLMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
	{
		if (severity is not (DebugSeverity.DebugSeverityHigh or DebugSeverity.DebugSeverityMedium))
			return;

		if (id == LastGLMsgCode)
			return;

		LastGLMsgCode = id;

		Log.Error("OpenGL", Encoding.Default.GetString((byte*)message, length));
	}

	internal static void Render()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		Sort();

		foreach (var renderable in Renderables)
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
		var ordered = Renderables.OrderBy(layer => layer.GetRenderLayer());
		Renderables.Clear();
		Renderables.AddRange(ordered);
	}
}
