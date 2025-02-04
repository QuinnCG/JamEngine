using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public class SpriteRenderer : Component
{
	private static readonly float[] _vertices =
	[
		-0.5f, -0.5f,
		-0.5f,  0.5f,
		 0.5f,  0.5f,
		 0.5f, -0.5f
	];
	private static readonly uint[] _indices =
	[
		0, 1, 2,
		3, 0, 2
	];

	private static int _vao, _vbo, _ibo;

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

	private RenderLayer _renderLayer = RenderLayer.Default;
	private readonly RenderHook _hook;

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
	}

	internal static void CleanUp()
	{
		GL.DeleteVertexArray(_vao);
		GL.DeleteBuffer(_vbo);
		GL.DeleteBuffer(_ibo);
	}

	protected override void OnCreate()
	{
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
		return (uint)_indices.Length;
	}

	private RenderLayer GetRenderLayer()
	{
		return _renderLayer;
	}
}
