using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Text;

namespace Engine.Rendering;

public static class Renderer
{
	public const string DebugLogCategory = "Renderer";

	public static Color4 ClearColor { get; set; } = Color4.Black;
	/// <summary>
	/// Called right after the <c>Renderer</c> initializes.
	/// </summary>
	public static event Action? OnInitialize;

	// Used to avoid spamming the same message in a row.
	// Set to -1 to avoid being equal to any debug message on first debug message.
	private static int _lastDebugMsgID = -1;
	private static readonly HashSet<IRenderable> _renderables = [];
	private static Shader? _defaultShader;

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
		GL.LoadBindings(new GLFWBindingsContext());

		GL.Enable(EnableCap.CullFace);
		GL.FrontFace(FrontFaceDirection.Cw);

		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		//GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

		GL.Enable(EnableCap.DebugOutput);
		GL.DebugMessageCallback(OnDebugMessageCallback, 0);

		// Initializes the default engine shader.
		using var stream = Resource.LoadFromEngine("Default.shader");
		Log.Assert(stream != null, "Missing default engine shader!");
		using var reader = new StreamReader(stream);
		_defaultShader = new Shader(reader.ReadToEnd());

		SpriteRenderer.Initialize();
		OnInitialize?.Invoke();
	}

	internal static void Clear()
	{
		GL.ClearColor(ClearColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);
	}

	internal static void Render()
	{
		Log.Assert(_defaultShader != null, "Default Shader should not be null!");

		foreach (var renderable in _renderables)
		{
			_defaultShader.Bind();
			_defaultShader.SetUniform("u_tint", renderable.Tint_Internal);

			var model = Matrix4.Identity;
			model *= Matrix4.CreateRotationZ(-renderable.Rotation_Internal.ToRadians());
			model *= Matrix4.CreateScale(renderable.Scale_Internal.ToVector3());
			model *= Matrix4.CreateTranslation(renderable.Position_Internal.ToVector3());

			var mvp = model * Camera.Active.GetMatrix();
			_defaultShader.SetUniform("u_mvp", mvp);

			_defaultShader.SetUniform("u_UVOffset", renderable.UVOffset_Internal);
			_defaultShader.SetUniform("u_UVScale", renderable.UVScale_Internal);

			renderable.Bind_Internal();
			GL.DrawElements(PrimitiveType.Triangles, renderable.IndexCount_Internal, DrawElementsType.UnsignedInt, 0);
		}
	}

	internal static void CleanUp()
	{
		_defaultShader?.Dispose();

		foreach (var renderable in _renderables)
		{
			renderable.CleanUp_Internal();
		}

		_renderables.Clear();
	}

	private static unsafe void OnDebugMessageCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
	{
		if (severity is not (DebugSeverity.DebugSeverityHigh or DebugSeverity.DebugSeverityMedium))
		{
			return;
		}

		if (id != _lastDebugMsgID)
		{
			_lastDebugMsgID = id;

			string encoded = Encoding.Default.GetString((byte*)message, length);
			Log.Error(DebugLogCategory, encoded);
		}
	}
}
