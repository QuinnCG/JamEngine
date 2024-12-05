using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Engine.Rendering;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex(Vector2 Position, Vector2 UV)
{
	/// <summary>
	/// The number of floats per vertex.
	/// </summary>
	internal const int FloatCount = 2 + 2;

	public Vector2 Position = Position;
	public Vector2 UV = UV;

	public static float[] GetRaw(Vertex[] vertices)
	{
		var raw = new float[FloatCount * vertices.Length];
		int vertexIndex = 0;

		for (int rawIndex = 0; rawIndex < raw.Length; rawIndex += FloatCount)
		{
			Vertex v = vertices[vertexIndex];
			vertexIndex++;

			// Position.
			raw[rawIndex + 0] = v.Position.X;
			raw[rawIndex + 1] = v.Position.Y;

			// UV.
			raw[rawIndex + 2] = v.UV.X;
			raw[rawIndex + 3] = v.UV.Y;
		}

		return raw;
	}
}
