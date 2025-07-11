using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Drawing;

namespace Engine.Rendering;

/// <summary>
/// A batch mesh of sprites.<br/><br/>
/// 
/// Usage:<br/>
/// <c>
///	- Clear();<br/>
///	- Sprite(...);<br/>
///	- Sprite(...);<br/>
///	- Generate();<br/>
/// - Renderer.Register(batch);
/// </c>
/// </summary>
public class SpriteBatch : IRenderable, IDisposable
{
	struct SpriteData
	{
		public Vector2 Position;
		public float Rotation;
		public Vector2 Scale;

		public Color4 Color;
		public float[] UVs;
	}

	public Texture Texture { get; set; }
	public RenderLayer RenderLayer { get; set; } = RenderLayer.Default;
	// HACK: [SpriteBatch.cs] Implement render bounds.
	public Bounds RenderBounds => new(default, Vector2.PositiveInfinity);

	public bool IsValid { get; private set; }

	public int VertexBufferSize { get; }
	public int IndexBufferSize { get; }

	private readonly List<SpriteData> _spritesToGenerate = [];
	private int _vao, _vbo, _ibo;

	private int _indexCount;

	/// <param name="bufferSize">The maximum number of bytes that the buffer can hold.</param>
	public SpriteBatch(int maxSpriteBuffer = 500)
	{
		// Pos, UV, Color
		int quadFloatCount = 2 + 2 + 4;
		int floatSize = sizeof(float);
		int indexCount = 6;

		VertexBufferSize = maxSpriteBuffer * (floatSize * quadFloatCount);
		IndexBufferSize = maxSpriteBuffer * indexCount;
	}

	/// <summary>
	/// Add a sprite to the batch. This will only take affect once <see cref="Generate"/> is called, again.
	/// </summary>
	/// <param name="pos">The world-space position of the center of the sprite.</param>
	/// <param name="rotation">The world-space rotation of the sprite.</param>
	/// <param name="scale">The world-space scale of the sprite.</param>
	/// <param name="color">The color tint of the sprite.</param>
	/// <param name="customUVs">Assign custom UVs to this sprite. Leave this emtpy to automatically use the default sprite UVs.</param>
	public void Sprite(Vector2 pos, float rotation, Vector2 scale, Color4 color, params float[] customUVs)
	{
		var sprite = new SpriteData()
		{
			Position = pos,
			Rotation = rotation,
			Scale = scale,

			Color = color,
			// Use custom UVs, if provided. Otherwise, use default UVs for a quad mesh.
			UVs = (customUVs == null || customUVs.Length == 0) ? [ 0f, 0f, 0f, 1f, 1f, 1f, 1f, 0f ] : customUVs
		};

		_spritesToGenerate.Add(sprite);
	}

	/// <summary>
	/// Generate/regenerate the internal mesh buffer.
	/// </summary>
	public void Generate()
	{
		// Generate the initial buffers.
		if (!IsValid)
		{
			IsValid = true;

			_vao = GL.GenVertexArray();
			GL.BindVertexArray(_vao);

			_vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, VertexBufferSize, 0, BufferUsageHint.DynamicDraw);

			_ibo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, IndexBufferSize, 0, BufferUsageHint.DynamicDraw);

			// Pos, UV, Color
			int stride = sizeof(float) * (2 + 2 + 4);

			// Pos
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);

			// UV
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);

			// Color
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, stride, sizeof(float) * 4);
		}

		// HACK: Couldn't we use arrays here?

		var allVertices = new List<float>();
		var allIndices = new List<uint>();

		uint greatestIndex = 0;

		for (int i = 0; i < _spritesToGenerate.Count; i++)
		{
			var sprite = _spritesToGenerate[i];

			float x = sprite.Position.X;
			float y = sprite.Position.Y;

			float w = sprite.Scale.X;
			float h = sprite.Scale.Y;

			float r = sprite.Color.R;
			float g = sprite.Color.G;
			float b = sprite.Color.B;
			float a = sprite.Color.A;

			var uvs = sprite.UVs;

			// TODO: [SpriteBatch.cs] Apply rotation to the vertices.

			var vertices = new float[]
			{
				// Pos								 // UV				 // Color
				(-0.5f * w) + x, (-0.5f * h) +  y,	 uvs[0], uvs[1],	 r, g, b, a,
				(-0.5f * w) + x, ( 0.5f * h) +  y,   uvs[2], uvs[3],     r, g, b, a,
				( 0.5f * w) + x, ( 0.5f * h) +  y,   uvs[4], uvs[5],     r, g, b, a,
				( 0.5f * w) + x, (-0.5f * h) +  y,   uvs[6], uvs[7],     r, g, b, a,
			};

			// ToArray() makes a copy of the indices.
			var indices = SpriteManager.Instance.Indices.ToArray();

			// Offset each index's value.
			for (int j = 0; j < indices.Length; j++)
			{
				ref var indexJ = ref indices[j];
				indexJ += greatestIndex;
			}

			// Update greatest index.
			foreach (uint j in indices)
			{
				if (j > greatestIndex)
				{
					greatestIndex = j + 1;
				}
			}

			allVertices.AddRange(vertices);
			allIndices.AddRange(indices);
		}

		_indexCount = allIndices.Count;

		GL.BindVertexArray(_vao);

		GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * allVertices.Count, allVertices.ToArray());
		GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, sizeof(uint) * allIndices.Count, allIndices.ToArray());
	}

	/// <summary>
	/// Clears all registered sprites. Called <see cref="Generate"/> after this will generate an empty batch.
	/// </summary>
	public void Clear()
	{
		_spritesToGenerate.Clear();
	}

	public void Dispose()
	{
		if (IsValid)
		{
			IsValid = false;
			_spritesToGenerate.Clear();

			GL.DeleteVertexArray(_vao);
			GL.DeleteBuffer(_vbo);
			GL.DeleteBuffer(_ibo);

			_indexCount = 0;
		}

		GC.SuppressFinalize(this);
	}

	public RenderLayer GetRenderLayer() => RenderLayer;

	public int Render()
	{
		SpriteManager.Instance.SpriteBatchShader.Bind();
		GL.BindVertexArray(_vao);

		var shader = SpriteManager.Instance.SpriteBatchShader;

		var mvp = CameraView.Current.ViewProjectionMatrix;
		shader.SetUniform("u_mvp", mvp);

		Texture?.Bind();
		shader.SetUniform("u_useTex", Texture != null);

		return _indexCount;
	}
}
