using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Engine.Rendering;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex(Vector2 Position, Vector2 UV)
{
	public const int ComponentCount = 2 + 2;

	public Vector2 Position = Position;
	public Vector2 UV = UV;

	public static float[] GetRaw(Vertex[] vertices)
	{
		var raw = new float[ComponentCount * vertices.Length];

		for (int i = 0; i < vertices.Length; i++)
		{
			Vertex v = vertices[i];

			// Position.
			raw[i + 0] = v.Position.X;
			raw[i + 1] = v.Position.Y;

			// UV.
			raw[i + 2] = v.UV.X;
			raw[i + 3] = v.UV.Y;
		}

		return raw;
	}
}
