using OpenTK.Mathematics;

namespace Engine;

public class Transform : Component, ITransform
{
	// TODO: Support position relative to parent.
	// Have local transform parts (pos, rot, scale) as well.
	// Allow opting out of relative transform on a per data basis (pos, rot, scale).

	public Vector2 Position { get; set; }
	public float Rotation { get; set; }
	public Vector2 Scale { get; set; }
	public float ScaleUniform
	{
		get => (Scale.X + Scale.Y) / 2f;
		set => Scale = new(value, value);
	}

	public void SetPositionX(float x) => Position = new(x, Position.Y);
	public void SetPositionY(float y) => Position = new(Position.X, y);
}
