namespace Engine.Rendering;

public record Shape
{
	public static readonly Shape Quad = new()
	{
		Vertices =
		[
			new(new(-0.5f, -0.5f), new(0f, 0f)),
			new(new(-0.5f,  0.5f), new(0f, 1f)),
			new(new( 0.5f,  0.5f), new(1f, 1f)),
			new(new( 0.5f, -0.5f), new(1f, 0f))
		],
		Indices =
		[
			0, 1, 2,
			3, 0, 2
		]
	};

	public Vertex[] Vertices { get; init; } = [];
	public uint[] Indices { get; init; } = [];
}
