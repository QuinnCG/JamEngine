namespace Engine;

public class World
{
	private static readonly HashSet<World> _worlds = [];

	public LifecycleState State { get; private set; } = LifecycleState.None;

	private readonly HashSet<Entity> _entities = [];

	internal static void LoadWorld(World world)
	{
		if (_worlds.Add(world))
		{
			world.Create();
		}
	}

	internal static void UpdateWorlds()
	{
		foreach (var world in _worlds)
		{
			world.Update();
		}
	}

	internal static void RenderWorlds()
	{
		foreach (var world in _worlds)
		{
			world.Render();
		}
	}

	internal static void UnloadWorld(World world)
	{
		if (_worlds.Remove(world))
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
				entity.Create();
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

	internal void Render()
	{
		if (State is LifecycleState.Created)
		{
			foreach (var entity in _entities)
			{
				entity.Render();
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
			ent.Create();
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
