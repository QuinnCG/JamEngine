using nkast.Aether.Physics2D.Dynamics;

namespace Engine.Simulation;

/// <summary>
/// The base class for all colliders. Colliders are paired with <see cref="Simulation.Rigidbody"/>s to simulate physics.
/// </summary>
public abstract class Collider : Component
{
	public const float DefaultDensity = 1f;

	public Rigidbody Rigidbody { get; private set; }
	public float Density
	{
		get => _density;
		set
		{
			_density = value;
			_fixture.Shape.Density = value;
		}
	}
	public float Friction
	{
		get => _fixture.Friction;
		set => _fixture.Friction = value;
	}
	/// <summary>
	/// Bounce coefficient.
	/// </summary>
	public float Restitution
	{
		get => _fixture.Restitution;
		set => _fixture.Restitution = value;
	}

	private Fixture _fixture;
	private float _density = DefaultDensity;

	protected override void OnStart()
	{
		if (TryGetComponent(out Rigidbody body))
		{
			Rigidbody = body;
			_fixture = CreateFixture(body.Body);

			_fixture.Friction = 1f;
		}
	}

	protected abstract Fixture CreateFixture(Body body);
}
