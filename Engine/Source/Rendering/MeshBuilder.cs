using OpenTK.Mathematics;

namespace Engine.Rendering;

public class MeshBuilder
{
	private readonly List<Vertex> _vertices = [];
	private readonly List<uint> _indices = [];

	private uint _indexOffset;

	/// <param name="rot">In degrees.</param>
	/// <param name="uvs">Custom UVs. Must be in quantity equal to vertex count or custom UVs will be ignored in favor of default UVs of the quad.</param>
	public MeshBuilder Quad(Vector2 pos, float rot, Vector2 scale, params Vector2[] uvs)
	{
		// Vertices.
		var vertices = Shape.Quad.Vertices.ToArray();

		for (int i = 0; i < vertices.Length; i++)
		{
			ref var vertex = ref vertices[i];

			vertex.Position *= scale;

			float mag = vertex.Position.Length;
			float baseRot = MathX.Atan2(vertex.Position.Y, vertex.Position.X);

			vertex.Position = new Vector2()
			{
				X = MathX.Sin(baseRot + rot.ToRadians()) * mag,
				Y = MathX.Cos(baseRot + rot.ToRadians()) * mag
			};

			vertex.Position += pos;
			
			if (uvs.Length == vertices.Length)
			{
				vertex.UV = uvs[i];
			}
		}

		_vertices.AddRange(vertices);


		// Indices.
		var indices = Shape.Quad.Indices.ToArray();

		for (int i = 0; i <  indices.Length; i++)
		{
			ref var index = ref indices[i];

			index += _indexOffset;
		}

		_indexOffset += (uint)vertices.Length;
		_indices.AddRange(indices);

		return this;
	}

	public Mesh Build(bool isStatic = true)
	{
		return new Mesh([.. _vertices], [.. _indices], isStatic);
	}
}
