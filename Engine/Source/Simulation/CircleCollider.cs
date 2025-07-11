using nkast.Aether.Physics2D.Dynamics;
using OpenTK.Mathematics;

namespace Engine.Simulation;

public class CircleCollider : Collider
{
	public Vector2 Offset { get; }
	public float Radius { get; }

	public CircleCollider() { }
	public CircleCollider(Vector2 offset, float radius)
	{
		Offset = offset;
		Radius = radius;
	}

	protected override Fixture CreateFixture(Body body)
	{
		float avg = MathF.Min(Scale.X, Scale.Y) + (MathX.Abs(Scale.X - Scale.Y) / 2f);
		return body.CreateCircle(Radius * avg, DefaultDensity, new(Offset.X, Offset.Y));
	}
}
