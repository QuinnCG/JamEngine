using nkast.Aether.Physics2D.Dynamics;
using OpenTK.Mathematics;

namespace Engine.Simulation;

public class PolygonCollider(Vector2[] vertices) : Collider
{
	public Vector2[] Vertices { get; } = vertices;

	protected override Fixture CreateFixture(Body body)
	{
		return body.CreatePolygon([.. Vertices.Select(v => new nkast.Aether.Physics2D.Common.Vector2(v.X, v.Y))], DefaultDensity);
	}
}
