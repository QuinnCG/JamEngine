namespace Engine;

public class World
{
	private static readonly HashSet<World> Worlds = [];

	public LifecycleState State { get; private set; } = LifecycleState.None;

	private readonly HashSet<Entity> _entities = [];

	public static void Load(World world)
	{
		if (Worlds.Add(world))
		{
			world.Create();
		}
	}

	internal static void UpdateAll()
	{
		foreach (var world in Worlds)
		{
			world.Update();
		}
	}

	public static void Unload(World world)
	{
		if (Worlds.Remove(world))
		{
			world.Destroy();
		}
	}

	internal void Create()
	{
		if (State is LifecycleState.None)
		{
			State = LifecycleState.Created;

			foreach (var entity in _entities)
			{
				entity.Create(this);
			}
		}
	}

	internal void Update()
	{
		if (State is LifecycleState.Created)
		{
			foreach (var entity in _entities)
			{
				entity.Update();
			}
		}
	}

	internal void Destroy()
	{
		if (State is LifecycleState.Created)
		{
			State = LifecycleState.Destroyed;

			foreach (var entity in _entities)
			{
				entity.Destroy();
			}
		}
	}

	public T CreateEntity<T>() where T : Entity, new()
	{
		var ent = new T();
		_entities.Add(ent);

		if (State is LifecycleState.Created)
		{
			ent.Create(this);
		}

		return ent;
	}

	public void DestroyEntity<T>(T entity) where T : Entity
	{
		if (_entities.Remove(entity) && State is LifecycleState.Created)
		{
			entity.Destroy();
		}
	}
}
