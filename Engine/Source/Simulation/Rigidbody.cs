using Engine.Source.Simulation;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using OpenTK.Mathematics;

namespace Engine.Simulation;

public class Rigidbody : Component, IPhysicsUpdateable
{
	/// <summary>
	/// • <c>Dynamic</c> - Positive mass, non-zero velocity determined by forces, moved by solver.<br/>
	/// • <c>Static</c> - Zero velocity, may be manually moved. Note: even static bodies have mass.<br/>
	/// • <c>Kinematic</c> - Zero mass, non-zero velocity set by user, moved by solver.
	/// </summary>
	public RigidbodyType BodyType { get; } = RigidbodyType.Dynamic;

	public Vector2 LinearVelocity
	{
		get => new(Body.LinearVelocity.X, Body.LinearVelocity.Y);
		set => Body.LinearVelocity = new(value.X, value.Y);
	}
	/// <summary>
	/// The mass of this rigidbody. This is 1, by default.
	/// </summary>
	public float Mass
	{
		get => Body.Mass;
		set => Body.Mass = value;
	}
	public float LinearDamping
	{
		get => Body.LinearDamping;
		set => Body.LinearDamping = value;
	}
	public float AngularVelocity
	{
		get => Body.AngularVelocity;
		set => Body.AngularVelocity = value;
	}
	public float AngularDamping
	{
		get => Body.AngularDamping;
		set => Body.AngularDamping = value;
	}
	/// <summary>
	/// If false, rotation won't be simulated, although manually setting the <see cref="Entity.Rotation"/> will still work.<br/>
	/// This is true, by default.
	/// </summary>
	public bool CanRotate
	{
		get => Body.FixedRotation;
		set
		{
			float mass = Body.Mass;
			Body.FixedRotation = value;
			Body.Mass = mass;
		}
	}

	/// <summary>
	/// The internal physics engine's representation of a rigidbody.<br/>
	/// This is used by <see cref="Rigidbody"/>, like a handle.
	/// </summary>
	internal Body Body { get; private set; }

	/// <summary>
	/// Passes the hit collider as input, and takes a return value, that, if false, will ignore the collision, altogether.
	/// </summary>
	public event Func<Collider, bool> OnCollide;

	/// <summary>
	/// Create a <see cref="RigidbodyType.Dynamic"/> body.
	/// </summary>
	public Rigidbody() { }
	public Rigidbody(RigidbodyType type)
	{
		BodyType = type;
	}

	protected override void OnCreate()
	{
		Entity.OnPositionChange += OnPosChanged;
		Entity.OnRotationChange += OnRotChanged;

		Body = World.PhysicsWorld.CreateBody(new(Position.X, Position.Y), Rotation, (BodyType)BodyType);
		Body.Mass = 1f;

		Body.OnCollision += OnCollision;
		World.RegisterRigidbody(this, Body);
	}

	public override string ToString()
	{
		return base.ToString() + $" <Type: {BodyType}, Mass: {Mass}>";
	}

	private bool OnCollision(Fixture sender, Fixture other, Contact contact)
	{
		var collider = World.GetCollider(other);

		// Don't collide with self.
		if (collider.Entity == Entity)
		{
			return false;
		}

		// Default behavior is to handle the collision.
		if (OnCollide == null)
		{
			return true;
		}
		// If there is an override event, then ask it if we should handle an event or not.
		// HACK: [Rigidbody.cs] This is an event, not a callback. Multiple subscribers could return different values; which one is selected?
		else
		{
			return OnCollide.Invoke(collider);
		}
	}

	public void PhysicsUpdate()
	{
		Entity.RawPosition = new(Body.Position.X, Body.Position.Y);
		Entity.RawRotation = Body.Rotation;
	}

	protected override void OnDestroy()
	{
		World.UnregisterRigidbody(Body);
		World.PhysicsWorld.Remove(Body);
	}

	private void OnPosChanged(Vector2 oldValue, Vector2 newValue)
	{
		Body.Position = new(Position.X, Position.Y);
	}

	private void OnRotChanged(float oldValue, float newValue)
	{
		Body.Rotation = Rotation;
	}
}
