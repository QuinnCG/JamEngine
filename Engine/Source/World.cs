using Engine.Simulation;
using OpenTK.Mathematics;
using Engine.Source.Simulation;

using AeWorld = nkast.Aether.Physics2D.Dynamics.World;
using AeBody = nkast.Aether.Physics2D.Dynamics.Body;
using AeFixture = nkast.Aether.Physics2D.Dynamics.Fixture;
using System.Text;

namespace Engine;

public class World
{
	public static World Current { get; private set; }

	public bool IsLoaded { get; private set; }
	/// <summary>
	/// The internal physics world.
	/// </summary>
	internal AeWorld PhysicsWorld { get; } = new();

	public IEnumerable<Entity> Entities => _entities;
	public int EntityCount => _entities.Count;

	private readonly HashSet<Entity> _entities = [];
	private readonly HashSet<Entity> _toCreate = [];
	private readonly HashSet<Entity> _toDestroy = [];

	private readonly HashSet<IPhysicsUpdateable> _physicsUpdateReceivers = [];
	private readonly Dictionary<AeBody, Rigidbody> _rigidbodies = [];
	private readonly Dictionary<AeFixture, Collider> _colliders = [];
	private float _nextPhysStepTime;

	private World() { }

	/// <summary>
	/// Contructs and returns a new world. You must still call <see cref="World.Load"/> yourself.
	/// </summary>
	/// <param name="gravity">A default value for gravity in this world. Only applies to <see cref="Rigidbody"/>s.</param>
	public static World Create(Vector2 gravity = default)
	{
		var world = new World();
		world.PhysicsWorld.Gravity = new(gravity.X, gravity.Y);

		return world;
	}

	public void Load()
	{
		if (!IsLoaded)
		{
			Current?.Unload();
			Current = this;
			IsLoaded = true;

			foreach (var entity in _entities)
			{
				entity.Create(this);
			}

			foreach (var entity in _entities)
			{
				entity.Start();
			}
		}
	}

	internal void RegisterRigidbody(Rigidbody rigidbody, AeBody body)
	{
		_rigidbodies.Add(body, rigidbody);
	}
	internal void UnregisterRigidbody(AeBody body)
	{
		_rigidbodies.Remove(body);
	}

	internal void RegisterCollider(Collider collider, AeFixture fixture)
	{
		_colliders.Add(fixture, collider);
	}
	internal void UnregisterCollider(AeFixture fixture)
	{
		_colliders.Remove(fixture);
	}

	/// <summary>
	/// Convert the internal physics engine's version of a rigidbody to the <see cref="Component"/> version.
	/// </summary>
	internal Rigidbody GetRigidybody(AeBody body)
	{
		return _rigidbodies[body];
	}
	internal Collider GetCollider(AeFixture fixture)
	{
		return _colliders[fixture];
	}

	internal void Update()
	{
		// Update physics world.
		if (Time.Now > _nextPhysStepTime)
		{
			_nextPhysStepTime = Time.Now + Physics.StepDelta;
			PhysicsWorld.Step(Physics.StepDelta);

			foreach (var physUpdate in _physicsUpdateReceivers)
			{
				physUpdate.PhysicsUpdate();
			}
		}

		// Update entities.
		foreach (var entity in _entities)
		{
			entity.Update();
		}
		foreach (var entity in _entities)
		{
			entity.LateUpdate();
		}

		// Add any deferred additions.
		foreach (var entity in _toCreate)
		{
			_entities.Add(entity);
			entity.Create(this);
		}
		foreach (var entity in _toCreate)
		{
			entity.Start();
		}

		// Remove any deferred removals.
		foreach (var entity in _toDestroy)
		{
			entity.Destroy();
			_entities.Remove(entity);
		}

		// Clear deferrment lists.
		_toCreate.Clear();
		_toDestroy.Clear();
	}

	/// <summary>
	/// Destroys all entities and sets the current world to null.
	/// </summary>
	public void Unload()
	{
		if (Current == this)
		{
			Current = null;
			IsLoaded = false;

			foreach (var entity in _entities)
			{
				entity.Destroy();
			}
		}
	}

	/// <summary>
	/// Constructs the entity of the specified type and adds it to this world.<br/>
	/// If the world is loaded, then it will be added at the end of the current frame.<br/>
	/// If the world is not loaded, then it will be added instantly, but <see cref="Entity.OnCreate"/> and <see cref="Entity.OnStart"/> won't be called until the world is loaded.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="Entity"/> and has a parameterless constructor.</typeparam>
	/// <returns>The constructed entity.</returns>
	public T CreateEntity<T>() where T : Entity, new()
	{
		var entity = new T();

		if (IsLoaded)
		{
			// If loaded, delay creating until next frame to avoid invalidating the entity update loop.
			_toCreate.Add(entity);
		}
		else
		{
			_entities.Add(entity);
		}

		var physicsUpdaters = entity.GetAllTypes<IPhysicsUpdateable>();
		_physicsUpdateReceivers.AddRange(physicsUpdaters);

		return entity;
	}

	public void DestroyEntity(Entity entity)
	{
		if (IsLoaded)
		{
			// If loaded, delay destroying until next frame to avoid invalidating the entity update loop.
			_toDestroy.Add(entity);
		}
		else
		{
			entity.Destroy();
			_entities.Remove(entity);
		}

		var physicsUpdaters = entity.GetAllTypes<IPhysicsUpdateable>();
		_physicsUpdateReceivers.RemoveRange(physicsUpdaters);
	}

	internal void RegisterPhyiscsCallback(IPhysicsUpdateable callback)
	{
		_physicsUpdateReceivers.Add(callback);
	}

	internal void UnregisterPhyiscsCallback(IPhysicsUpdateable callback)
	{
		_physicsUpdateReceivers.Remove(callback);
	}

	public void LogHierarchy()
	{
		var builder = new StringBuilder();

		builder.AppendLine("World:");

		foreach (var entity in _entities)
		{
			builder.AppendLine($"  - {entity}:");

			foreach (var component in entity.GetAllComponents<Component>())
			{
				builder.AppendLine($"    - {component}");
			}

			builder.AppendLine();
		}

		if (_entities.Count == 0)
		{
			builder.AppendLine("  - No Entites Spawned");
		}

		Log.Info(builder.ToString());
	}
}
