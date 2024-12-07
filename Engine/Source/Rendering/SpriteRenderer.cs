using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Engine.Rendering;

// TODO: Make IRenderable a class instance and create and hold it internally?
// How to handle data update? Callback or OnRender method?

[DependOn<Transform>]
public class SpriteRenderer : Component, IRenderable
{
	private static int _vao, _vbo, _ibo;

	public Sprite Sprite { get; set; }
	public Color4 Tint { get; set; } = Color4.White;

	private Transform? _transform;

	Vector2 IRenderable.Position => _transform!.Position;
	float IRenderable.Rotation => _transform!.Rotation;
	Vector2 IRenderable.Scale => _transform!.Scale;

	Texture? IRenderable.Texture => Sprite.Texture;
	Vector2 IRenderable.UVOffset => Sprite.UVOffset;
	Vector2 IRenderable.UVScale => Sprite.UVScale;

	Color4 IRenderable.Tint => Tint;
	int IRenderable.IndexCount => 6;

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

	protected override void OnCreate()
	{
		_transform = GetComponent<Transform>();
		Renderer.Register(this);
	}

	protected override void OnDestroy()
	{
		Renderer.Unregister(this);
	}

	void IRenderable.Bind()
	{
		GL.BindVertexArray(_vao);
	}

	void IRenderable.CleanUp()
	{
		GL.DeleteVertexArray(_vao);
		GL.DeleteBuffer(_vbo);
		GL.DeleteBuffer(_ibo);
	}
}
