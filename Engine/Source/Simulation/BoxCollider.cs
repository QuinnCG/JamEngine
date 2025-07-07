using nkast.Aether.Physics2D.Dynamics;
using OpenTK.Mathematics;

namespace Engine.Simulation;

public class BoxCollider : Collider
{
	public Vector2 Offset { get; }
	public Vector2 Size { get; }

	/// <summary>
	/// Use a size of (1, 1), scaled by the Entity's Scale property.
	/// </summary>
	public BoxCollider() { }
	public BoxCollider(Vector2 offset, Vector2 size)
	{
		Offset = offset;
		Size = size;
	}

	protected override Fixture CreateFixture(Body body)
	{
		return body.CreateRectangle(Size.X, Size.Y, DefaultDensity, new(Offset.X, Offset.Y));
	}
}
