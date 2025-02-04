using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

public class SpriteRenderer : Component
{
	private static readonly float[] _vertices =
	[
		-0.5f, -0.5f,	0f, 0f,
		-0.5f,  0.5f,	0f, 1f,
		 0.5f,  0.5f,	1f, 1f,
		 0.5f, -0.5f,	1f, 0f
	];
	private static readonly uint[] _indices =
	[
		0, 1, 2,
		3, 0, 2
	];

	private static readonly string _shaderPath = "DefaultSprite.shader";

	private static int _vao, _vbo, _ibo;
	private static Shader? _shader;

	public RenderLayer RenderLayer
	{
		get => _renderLayer;
		set
		{
			if (_renderLayer != value)
			{
				_renderLayer = value;
				_hook.NotifyLayerChange();
			}
		}
	}
	/// <summary>
	/// The alpha channel currently does nothing for tint.
	/// </summary>
	public Color4 Tint { get; set; } = Color4.White;

	private RenderLayer _renderLayer = RenderLayer.Default;
	private readonly RenderHook _hook;

	private SpatialEntity? _entity;

	public SpriteRenderer()
	{
		_hook = new RenderHook(OnBind, GetRenderLayer);
	}

	internal static void Initialize()
	{
		_vao = GL.GenVertexArray();
		GL.BindVertexArray(_vao);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * _vertices.Length, _vertices, BufferUsageHint.StaticDraw);

		_ibo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * _indices.Length, _indices, BufferUsageHint.StaticDraw);

		int stride = sizeof(float) * 4;

		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);

		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);

		_shader = Resource.LoadEngineResource<Shader>(_shaderPath);
	}

	internal static void CleanUp()
	{
		GL.DeleteVertexArray(_vao);
		GL.DeleteBuffer(_vbo);
		GL.DeleteBuffer(_ibo);

		_shader!.Release();
		_shader = null;
	}

	protected override void OnCreate()
	{
		if (Entity is SpatialEntity ent)
		{
			_entity = ent;
		}
		else
		{
			Log.Error($"SpriteRenderer on entity '{Entity.Name}' must have a SpatialEntity!");
			return;
		}

		Renderer.RegisterHook(_hook);
	}

	protected override void OnDestroy()
	{
		Renderer.UnregisterHook(_hook);
	}

	private uint OnBind()
	{
		if (!DoesUpdate)
		{
			return 0;
		}

		GL.BindVertexArray(_vao);

		var mvp = Matrix4.Identity;
		mvp *= Matrix4.CreateTranslation(new Vector3(_entity!.WorldPosition));

		_shader!.Bind();
		_shader!.SetUniform("u_color", Tint);
		_shader!.SetUniform("u_mvp", mvp);

		return (uint)_indices.Length;
	}

	private RenderLayer GetRenderLayer()
	{
		return _renderLayer;
	}
}
