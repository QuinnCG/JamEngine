namespace Engine;

public class World
{
	public static World Current { get; private set; }

	public bool IsLoaded { get; private set; }

	private readonly HashSet<Entity> _entities = [];
	private readonly HashSet<Entity> _toCreate = [];
	private readonly HashSet<Entity> _toDestroy = [];

	private World() { }

	public static World Create() => new();

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
		}
	}

	internal void Update()
	{
		// Update entities.
		foreach (var entity in _entities)
		{
			entity.Update();
		}

		// Add any deferred additions.
		foreach (var entity in _toCreate)
		{
			_entities.Add(entity);
			entity.Create(this);
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
			entity.Create(this);
			_entities.Add(entity);
		}

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
	}
}
