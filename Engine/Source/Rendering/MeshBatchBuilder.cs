using OpenTK.Mathematics;

namespace Engine.Rendering;

public class MeshBatchBuilder
{
	private readonly List<float> _vertices = [];
	private readonly List<uint> _indices = [];

	private bool _generateUVs;
	private int _greatestIndex = -1;

	private MeshBatchBuilder() { }

	public static MeshBatchBuilder Create(bool generateUVs = true)
	{
		return new MeshBatchBuilder()
		{
			_generateUVs = generateUVs
		};
	}

	public MeshBatchBuilder Quad(Vector2 pos, Vector2 size, params float[] customData)
	{
		float[] GenerateVertex(Vector2 pos, Vector2 uv)
		{
			if (_generateUVs)
			{
				return [pos.X, pos.Y, uv.X, uv.Y, .. customData];
			}
			else
			{
				return [pos.X, pos.Y, .. customData];
			}
		}

		// Vertices.
		Vector2 half = size / 2f;
		Vector2 lower = pos - half;
		Vector2 upper = pos + half;
		Vector2 topLeft = new(lower.X, upper.Y);
		Vector2 botRight = new(upper.X, lower.Y);

		_vertices.AddRange(GenerateVertex(lower, new(0f, 0f)));
		_vertices.AddRange(GenerateVertex(topLeft, new(0f, 1f)));
		_vertices.AddRange(GenerateVertex(upper, new(1f, 1f)));
		_vertices.AddRange(GenerateVertex(botRight, new(1f, 0f)));

		// Indices.
		_greatestIndex++;
		uint index = (uint)_greatestIndex;

		_indices.AddRange([
			index + 0,
			index + 1,
			index + 2,
			index + 3,
			index + 0,
			index + 2,
			]);

		return this;
	}

	public void Build(out float[] vertices, out uint[] indices)
	{
		vertices = [.. _vertices];
		indices = [.. _indices];
	}
}
