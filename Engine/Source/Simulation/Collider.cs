using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace Engine.Simulation;

/// <summary>
/// The base class for all colliders. Colliders are paired with <see cref="Simulation.Rigidbody"/>s to simulate physics.
/// </summary>
public abstract class Collider : Component
{
	public const float DefaultDensity = 1f;

	public Rigidbody Rigidbody { get; private set; }
	/// <summary>
	/// By default, this is 1.
	/// </summary>
	public float Density
	{
		get => _density;
		set
		{
			_density = value;
			_fixture.Shape.Density = value;
		}
	}
	/// <summary>
	/// By default, this is 1.
	/// </summary>
	public float Friction
	{
		get => _fixture.Friction;
		set => _fixture.Friction = value;
	}
	/// <summary>
	/// The bounce coefficient. By default, this is 0.
	/// </summary>
	public float Restitution
	{
		get => _fixture.Restitution;
		set => _fixture.Restitution = value;
	}
	/// <summary>
	/// Triggers do not affect other colliders. They are made aware of when they overlap with other colliders, though.
	/// </summary>
	public bool IsTrigger
	{
		get => _fixture.IsSensor;
		set => _fixture.IsSensor = value;
	}
	/// <summary>
	/// This defaults to all.
	/// </summary>
	public CollisionLayer Layer
	{
		get => (CollisionLayer)_fixture.CollisionCategories;
		set => _fixture.CollisionCategories = (Category)value;
	}
	/// <summary>
	/// This defaults to all.
	/// </summary>
	public CollisionLayer CollidesWith
	{
		get => (CollisionLayer)_fixture.CollidesWith;
		set => _fixture.CollidesWith = (Category)value;
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
			_fixture.Restitution = 0f;

			_fixture.CollisionCategories = Category.All;
			_fixture.CollidesWith = Category.All;

			World.RegisterCollider(this, _fixture);
		}
	}

	protected override void OnDestroy()
	{
		World.UnregisterCollider(_fixture);
	}

	protected abstract Fixture CreateFixture(Body body);
}
