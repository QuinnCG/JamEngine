using OpenTK.Mathematics;

namespace Engine.Rendering;

public class RenderObject
{
	public Func<bool> IsInvisible { get; init; } = () => false;
	public required Action OnBind { get; init; }
	public Action OnDispose { get; init; } = () => { };

	// FIX: Is OnDipose needed?

	public required Func<int> GetIndexCount { get; init; }

	public Func<bool> IsScreenSpace { get; init; } = () => false;
	public Func<Vector2> GetPosition { get; init; } = () => Vector2.Zero;
	public Func<float> GetRotation { get; init; } = () => 0f;
	public Func<Vector2> GetScale { get; init; } = () => Vector2.One;

	public Func<Color4> GetTint { get; init; } = () => Color4.White;

	public Func<Texture?> GetTexture { get; init; } = () => null;
	public Func<Vector2> GetUVOffset { get; init; } = () => Vector2.Zero;
	public Func<Vector2> GetUVScale { get; init; } = () => Vector2.One;
}
