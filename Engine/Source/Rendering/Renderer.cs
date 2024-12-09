using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.InteropServices;
using System.Text;

namespace Engine.Rendering;

public static class Renderer
{
	public const string DebugLogCategory = "Renderer";

	/// <summary>
	/// The color of the background.
	/// <br>This is the clear color used by the <c>Renderer</c>.</br>
	/// </summary>
	public static Color4 BackgroundColor { get; set; } = Color4.Black;

	/// <summary>
	/// Called right after the <c>Renderer</c> initializes.
	/// </summary>
	public static event Action? OnInitialize;

	// Used to avoid spamming the same message in a row.
	// Set to -1 to avoid being equal to any debug message on first debug message.
	private static int _lastDebugMsgID = -1;
	private static readonly HashSet<RenderObject> _renderObjects = [];
	private static Shader? _defaultShader;

	private static int _deferredTextureObj;
	private static int _deferredFrameObj;
	private static Shader? _deferredShader;
	private static int _deferredVAO, _deferredVBO, _deferredIBO;

	public static void Register(RenderObject renderObj)
	{
		_renderObjects.Add(renderObj);
	}

	public static void Unregister(RenderObject renderObj)
	{
		_renderObjects.Remove(renderObj);
	}

	internal static void Initialize()
	{
		Window.OnWindowResize += OnWindowResize;


		// Initialize OpenGL.
		GL.LoadBindings(new GLFWBindingsContext());

		// OpenGL settings.
		GL.Enable(EnableCap.CullFace);
		GL.FrontFace(FrontFaceDirection.Cw);

		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		GL.Enable(EnableCap.DebugOutput);
		GL.DebugMessageCallback(OnDebugMessageCallback, 0);

		// Initializes the default shader.
		using var stream = Resource.LoadFromEngine("Default.shader");
		Log.Assert(stream != null, "Missing default engine shader!");
		using var reader = new StreamReader(stream);
		_defaultShader = new Shader(reader.ReadToEnd());

		// Initialize the sprite rendering system.
		SpriteRenderer.Initialize();

		//InitializeDeferredFramebuffer();


		// Invoke client code.
		OnInitialize?.Invoke();
	}

	// TODO: Support renderable sorting.
	// Layers -> Index -> Order in Hierarchy

	internal static void Render()
	{
		Log.Assert(_defaultShader != null, "Default shader should not be null!");
		//Log.Assert(_deferredShader != null, "Deferred shader should not be null!");

		// Bind deferred framebuffer 
		// The actual scene will be rendered to this.
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, _deferredFrameObj);

		// Clear framebuffer.
		GL.ClearColor(BackgroundColor);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		// Bind default shader.
		_defaultShader.Bind();

		// Render each renderable.
		foreach (var renderObj in _renderObjects)
		{
			if (renderObj.IsInvisible())
			{
				continue;
			}

			// Set shader tint.
			_defaultShader.SetUniform("u_tint", renderObj.GetTint());
			_defaultShader.SetUniform("u_isTextured", renderObj.GetTexture() != null);
			_defaultShader.SetUniform("u_discardBlack", renderObj.DiscardBlack());

			// Calculate model for MVP.
			var model = Matrix4.Identity;
			model *= Matrix4.CreateRotationZ(-renderObj.GetRotation().ToRadians());
			model *= Matrix4.CreateScale(renderObj.GetScale().ToVector3());
			model *= Matrix4.CreateTranslation(renderObj.GetPosition().ToVector3());

			// Shader MVP.
			var mvp = model * Camera.Active.GetMatrix(renderObj.IsScreenSpace());
			_defaultShader.SetUniform("u_mvp", mvp);

			// Shader UV.
			_defaultShader.SetUniform("u_UVOffset", renderObj.GetUVOffset());
			_defaultShader.SetUniform("u_UVScale", renderObj.GetUVScale());


			// Bind mesh data and draw to buffer.
			renderObj.OnBind();
			GL.DrawElements(PrimitiveType.Triangles, renderObj.GetIndexCount(), DrawElementsType.UnsignedInt, 0);
		}


		// UNDONE: Implement deferred rendering or remove all commented code relating to it in this file.

		//// Unbind deferred framebuffer so that we can now render deferred quad to screen.
		//GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

		//// Clear screen.
		//GL.ClearColor(0f, 0f, 0f, 1f);
		//GL.Clear(ClearBufferMask.ColorBufferBit);

		//GL.BindVertexArray(_deferredVAO);
		//GL.BindTexture(TextureTarget.Texture2D, _deferredTextureObj);
		//_deferredShader.Bind();

		//// Render to screen.
		//GL.DrawElements(PrimitiveType.Triangles, Shape.Quad.Indices.Length, DrawElementsType.UnsignedInt, 0);


		//// Unbind GL objects used to render deferred quad to the screen.
		//GL.BindVertexArray(0);
		//GL.BindTexture(TextureTarget.Texture2D, 0);
		//GL.UseProgram(0);
	}

	internal static void CleanUp()
	{
		// Dispose of GL objects and left-over renderables.
		_defaultShader?.Dispose();
		foreach (var renderable in _renderObjects)
		{
			renderable.OnDispose();
		}
		_renderObjects.Clear();

		SpriteRenderer.CleanUp();


		// Dispose of GL objects used for deferred rendering.
		//GL.DeleteVertexArray(_deferredVAO);
		//GL.DeleteBuffer(_deferredVBO);
		//GL.DeleteBuffer(_deferredIBO);
		//GL.DeleteTexture(_deferredTextureObj);
		//_deferredShader?.Dispose();
		//GL.DeleteFramebuffer(_deferredFrameObj);
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

			if (severity is DebugSeverity.DebugSeverityHigh)
			{
				Log.Error(DebugLogCategory, encoded);
			}
			else
			{
				Log.Warning(DebugLogCategory, encoded);
			}
		}
	}

	private static void OnWindowResize(Vector2i size)
	{
		GL.Viewport(0, 0, size.X, size.Y);
		//UpdateDeferredFrameBufferTexture();
	}

	private static void InitializeDeferredFramebuffer()
	{
		// Set up deferred rendering shader.
		using var dStream = Resource.LoadFromEngine("Deferred.shader");
		using var dReader = new StreamReader(dStream);
		_deferredShader = new Shader(dReader.ReadToEnd());

		// Render target texture.
		_deferredTextureObj = GL.GenTexture();
		UpdateDeferredFrameBufferTexture();
		GL.TextureParameter(_deferredTextureObj, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
		GL.TextureParameter(_deferredTextureObj, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

		// Framebuffer to render the scene to.
		_deferredFrameObj = GL.GenFramebuffer();
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, _deferredFrameObj);
		GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _deferredTextureObj, 0);

		// Mesh for screen quad.
		_deferredVAO = GL.GenVertexArray();
		GL.BindVertexArray(_deferredVAO);

		Vertex[] vertices = Shape.Quad.Vertices;
		uint[] indices = Shape.Quad.Indices;

		_deferredVBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _deferredVBO);
		GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf<Vertex>() * vertices.Length, Vertex.GetRaw(vertices), BufferUsageHint.StaticDraw);

		_deferredIBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _deferredIBO);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

		// Set the attribute layout for screen quad.
		Vertex.SetGLLayout();

		// Unbind deferred rendering GL objects.
		GL.BindVertexArray(0);
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}

	private static void UpdateDeferredFrameBufferTexture()
	{
		Vector2i size = Window.Size;

		GL.BindTexture(TextureTarget.Texture2D, _deferredTextureObj);
		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, size.X, size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, 0);
	}
}
