using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Engine.Rendering;

[DependOn<Transform>]
public class SpriteRenderer : Component, IRenderable
{
	private readonly static Vertex[] _vertices =
		[
			// Position.			// UV.
			new(new(-0.5f, -0.5f),  new(0f, 0f)),
			new(new(-0.5f,  0.5f),  new(0f, 1f)),
			new(new( 0.5f,  0.5f),  new(1f, 1f)),
			new(new( 0.5f, -0.5f),  new(1f, 0f))
		];
	private readonly static uint[] _indices =
		[
			0, 1, 2,
			3, 0, 2
		];
	private static int _vao, _vbo, _ibo;

	// TODO: This should be a sprite that references a texture and holds UV data.
	public Texture? Texture { get; set; }
	public Color4 Tint { get; set; } = Color4.White;

	private Transform? _transform;

	Vector2 IRenderable.Position => _transform!.Position;
	float IRenderable.Rotation => _transform!.Rotation;
	Vector2 IRenderable.Scale => _transform!.Scale;

	Color4 IRenderable.Tint => Tint;
	int IRenderable.IndexCount => 6;

	internal static void Initialize()
	{
		_vao = GL.GenVertexArray();
		GL.BindVertexArray(_vao);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf<Vertex>() * _vertices.Length, Vertex.GetRaw(_vertices), BufferUsageHint.StaticDraw);

		_ibo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * _indices.Length, _indices, BufferUsageHint.StaticDraw);

		int stride = sizeof(float) * Vertex.FloatCount;

		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);

		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);
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
