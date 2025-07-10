using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

public static class Renderer
{
	public static Color4 ClearColor { get; set; } = Color4.Black;

	private static HashSet<IRenderable> _renderables = [];
	private static int _lastGLMsgCode = -1;

	public static void Register(IRenderable renderable)
	{
		_renderables.Add(renderable);
	}

	public static void Unregister(IRenderable renderable)
	{
		_renderables.Remove(renderable);
	}

	internal static void Initialize()
	{
		GL.Enable(EnableCap.DebugOutput);
		GL.DebugMessageCallback(OnGLMessage, 0);

		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
	}

	private static unsafe void OnGLMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
	{
		if (severity is not (DebugSeverity.DebugSeverityHigh or DebugSeverity.DebugSeverityMedium))
			return;

		if (id == _lastGLMsgCode)
			return;

		_lastGLMsgCode = id;

		Log.Error("OpenGL", Encoding.Default.GetString((byte*)message, length));
	}

	internal static void Render()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		Sort();

		var viewBounds = CameraView.Current.ViewBounds;

		int rendered = 0;

		foreach (var renderable in _renderables)
		{
			// Cull render objects not in view.
			if (viewBounds.IsNotTouchingBounds(renderable.RenderBounds))
			{
				continue;
			}

			rendered++;

			// Unbind any active texture.
			GL.BindTexture(TextureTarget.Texture2D, 0);
			// Execute the render code for the given render object.
			int indexCount = renderable.Render();

			if (indexCount > 0)
			{
				GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
			}
		}
	}

	private static void Sort()
	{
		_renderables = [.. _renderables.OrderBy(layer => layer.GetRenderLayer())];
	}
}
