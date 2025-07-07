using Engine.Source.Simulation;
using nkast.Aether.Physics2D.Dynamics;
using OpenTK.Mathematics;

namespace Engine.Simulation;

public class Rigidbody : Component, IPhysicsUpdateable
{
	public RigidbodyType BodyType { get; } = RigidbodyType.Dynamic;

	public Vector2 LinearVelocity
	{
		get => new(Body.LinearVelocity.X, Body.LinearVelocity.Y);
		set => Body.LinearVelocity = new(value.X, value.Y);
	}
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

	internal Body Body { get; private set; }

	public Rigidbody() { }
	public Rigidbody(RigidbodyType type)
	{
		BodyType = type;
	}

	protected override void OnCreate()
	{
		Body = World.PhysicsWorld.CreateBody(new(Position.X, Position.Y), Rotation, (BodyType)BodyType);
		Body.Mass = 1f;

		World.RegisterRigidbody(this, Body);
	}

	public void PhysicsUpdate()
	{
		Position = new(Body.Position.X, Body.Position.Y);
		Rotation = Body.Rotation;
	}

	protected override void OnLateUpdate()
	{
		Body.Position = new(Position.X, Position.Y);
		Body.Rotation = Rotation;
	}

	protected override void OnDestroy()
	{
		World.UnregisterRigidbody(Body);
		World.PhysicsWorld.Remove(Body);
	}
}
