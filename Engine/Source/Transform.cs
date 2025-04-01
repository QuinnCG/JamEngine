using OpenTK.Mathematics;

namespace Engine;

public class Transform : Component
{
	public Vector2 Position { get; set; }
	public float Rotation { get; set; }
	public Vector2 Scale { get; set; } = Vector2.One;
}
