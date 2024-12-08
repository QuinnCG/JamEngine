using OpenTK.Mathematics;

namespace Engine.Rendering;

public class RenderObject
{
	public Func<bool> IsInvisible { get; init; } = () => false;
	public required Action OnBind { get; init; }
	public required Action OnDispose { get; init; }

	public required Func<int> GetIndexCount { get; init; }

	public Func<bool> IsScreenSpace { get; init; } = () => false;
	public required Func<Vector2> GetPosition { get; init; }
	public required Func<float> GetRotation { get; init; }
	public required Func<Vector2> GetScale { get; init; }

	public required Func<Color4> GetTint { get; init; }

	public required Func<Texture?> GetTexture { get; init; }
	public required Func<Vector2> GetUVOffset { get; init; }
	public required Func<Vector2> GetUVScale { get; init; }
}
