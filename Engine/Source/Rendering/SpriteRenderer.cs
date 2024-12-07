using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Engine.Rendering;

// TODO: Make IRenderable a class instance and create and hold it internally?
// How to handle data update? Callback or OnRender method?

[DependOn<Transform>]
public class SpriteRenderer : Component
{
	private static int _vao, _vbo, _ibo;

	public Sprite? Sprite { get; set; }
	public Color4 Tint { get; set; } = Color4.White;

	private Transform? _transform;

	private readonly RenderObject _renderObj;

	internal static void Initialize()
	{
		var vertices = Shape.Quad.Vertices;
		var indices = Shape.Quad.Indices;

		_vao = GL.GenVertexArray();
		GL.BindVertexArray(_vao);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf<Vertex>() * vertices.Length, Vertex.GetRaw(vertices), BufferUsageHint.StaticDraw);

		_ibo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

		Vertex.SetGLLayout();
		GL.BindVertexArray(0);
	}

	internal static void CleanUp()
	{
		GL.DeleteVertexArray(_vao);
		GL.DeleteBuffer(_vbo);
		GL.DeleteBuffer(_ibo);
	}

	public SpriteRenderer()
	{
		_renderObj = new RenderObject()
		{
			OnBind = OnBind,
			OnDispose = OnDispose,

			GetIndexCount = () => Shape.Quad.Indices.Length,

			GetPosition = () => _transform!.Position,
			GetRotation = () => _transform!.Rotation,
			GetScale = () => _transform!.Scale,

			GetTint = () => Tint,

			GetTexture = () => Sprite?.Texture,
			GetUVOffset = () => Sprite.HasValue ? Sprite.Value.UVOffset : Vector2.Zero,
			GetUVScale = () => Sprite.HasValue ? Sprite.Value.UVScale : Vector2.One
		};
	}

	private void OnBind()
	{
		GL.BindVertexArray(_vao);
	}

	/// <summary>
	/// Clean up is handled by static <c>SpriteRenderer.CleanUp()</c> method.
	/// </summary>
	private void OnDispose() { }

	protected override void OnCreate()
	{
		_transform = GetComponent<Transform>();
		Renderer.Register(_renderObj);
	}

	protected override void OnDestroy()
	{
		Renderer.Unregister(_renderObj);
	}
}
