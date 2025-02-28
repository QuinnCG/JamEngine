using Engine.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Text;

namespace Engine.Rendering;

public static class Renderer
{
	public const string LogCategory = "Renderer";

	// TODO: Implement framerate. Literal framerate but also an averaged one that uses velocity to slowly update to actual framework to avoid rapid jitter.
	public static int Framerate { get; private set; }
	public static Color4 ClearColor { get; set; } = Color4.Black;

	private static HashSet<RenderHook> _hooks = [];
	private static int _glDebugCallbackID = -1;

	public static void RegisterHook(RenderHook hook)
	{
		_hooks.Add(hook);
		SortHooks();

		hook.OnLayerChange += SortHooks;
	}

	public static void UnregisterHook(RenderHook hook)
	{
		if (_hooks.Remove(hook))
		{
			SortHooks();
			hook.OnLayerChange -= SortHooks;
		}
	}

	internal static void Initialize()
	{
		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		GL.DebugMessageCallback(GLDebugCallback, 0);
		SpriteRenderer.Initialize();
		Image.Initialize();
	}

	private static unsafe void GLDebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
	{
		if (id != _glDebugCallbackID && severity is not DebugSeverity.DontCare or DebugSeverity.DebugSeverityLow or DebugSeverity.DebugSeverityNotification)
		{
			string msg = Encoding.Default.GetString((byte*)message, length);
			_glDebugCallbackID = id;
			
			if (severity is DebugSeverity.DebugSeverityMedium)
			{
				Log.Warning(LogCategory, msg);
			}
			else if (severity is DebugSeverity.DebugSeverityHigh)
			{
				Log.Error(LogCategory, msg);
			}
		}
	}

	internal static void Flush()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		foreach (var hook in _hooks)
		{
			int indices = hook.Bind();
			
			// Skip indicies if value is <= 0.
			if (indices > 0)
			{
				GL.DrawElements(PrimitiveType.Triangles, indices, DrawElementsType.UnsignedInt, 0);
			}
		}
	}

	internal static void CleanUp()
	{
		SpriteRenderer.CleanUp();
		Image.CleanUp();
	}

	// Sort registered render hooks by their render layers.
	private static void SortHooks()
	{
		_hooks = [.. _hooks.OrderBy(x => x.Layer().Order)];
	}
}
