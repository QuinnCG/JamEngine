using OpenTK.Graphics.OpenGL4;
using System.Runtime.Versioning;

namespace Engine.Rendering;

/// <summary>
/// Creates and maanges the lifecycle of a static sprite mesh.<br/>
/// Used for non-batched sprite rendering.
/// </summary>
public class SpriteManager : GlobalManager
{
	public static SpriteManager Instance { get; private set; }

	/// <summary>
	/// Position, UV
	/// </summary>
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

	/// <summary>
	/// The shader resource used by <see cref="SpriteBatch"/> instances.
	/// </summary>
	internal Shader SpriteBatchShader { get; private set; }

	private int _spriteMeshVAO;
	private int _vbo, _ibo;

	/// <summary>
	/// Binds the static sprite mesh's GL state for rendering.
	/// </summary>
	/// <returns>The number of indices for the sprite. This will always be 6.</returns>
	public int BindSpriteMesh()
	{
		GL.BindVertexArray(_spriteMeshVAO);
		return Indices.Length;
	}

	protected override void OnBegin()
	{
		SpriteBatchShader = Resource.LoadEngineResource<Shader>("BatchSprite.shader");
		Instance = this;

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

		Instance = null;
		SpriteBatchShader.Release();
	}
}
