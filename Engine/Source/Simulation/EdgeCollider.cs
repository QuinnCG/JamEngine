using nkast.Aether.Physics2D.Dynamics;
using OpenTK.Mathematics;

namespace Engine.Simulation;

public class EdgeCollider(Vector2 start, Vector2 end) : Collider
{
	public Vector2 StartPoint { get; } = start;
	public Vector2 EndPoint { get; } = end;

	protected override Fixture CreateFixture(Body body)
	{
		return body.CreateEdge(new(StartPoint.X, StartPoint.Y), new(StartPoint.X, StartPoint.Y));
	}
}
