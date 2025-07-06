using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

/// <summary>
/// Creates and maanges the lifecycle of a static sprite mesh.<br/>
/// Used for non-batched sprite rendering.
/// </summary>
public class SpriteManager : GlobalManager
{
	public float[] Vertices =
	[
		-0.5f, -0.5f,   0f, 0f,
		-0.5f,  0.5f,   0f, 1f,
		 0.5f,  0.5f,   1f, 1f,
		 0.5f, -0.5f,   1f, 0f
	];

	public uint[] Indices =
	[
		0, 1, 2,
		3, 0, 2
	];

	private int _spriteMeshVAO;
	private int _vbo, _ibo;

	public void BindSpriteMesh()
	{
		GL.BindVertexArray(_spriteMeshVAO);
	}

	protected override void OnBegin()
	{
		_spriteMeshVAO = GL.GenVertexArray();
		GL.BindVertexArray(_spriteMeshVAO);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * Vertices.Length, Vertices, BufferUsageHint.StaticDraw);

		_ibo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * Indices.Length, Indices, BufferUsageHint.StaticDraw);

		int stride = sizeof(float) * 4;

		// Pos
		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);

		// UV
		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);
	}

	protected override void OnEnd()
	{
		GL.DeleteVertexArray(_spriteMeshVAO);
		GL.DeleteBuffer(_vbo);
		GL.DeleteBuffer(_ibo);
	}
}
