namespace Engine;

public class World
{
	public static IEnumerable<World> LoadedWorlds => _loadedWorlds;
	private static readonly HashSet<World> _loadedWorlds = [];

	public bool IsLoaded { get; private set; }

	private readonly HashSet<Entity> _rootEntities = [];

	private readonly HashSet<Entity> _allEntities = [];
	private readonly HashSet<Entity> _enabledEntities = [];

	private readonly Dictionary<Type, HashSet<Entity>> _typeToEntity = [];
	private readonly Dictionary<Type, HashSet<Entity>> _tagToEntity = [];

	// Entities to be destroyed after updating all entities.
	private readonly HashSet<Entity> _toDestroy = [];

	public static void Load(World world)
	{
		world.Load_Internal();
	}
	public static void Load(/*WorldSave res?*/)
	{
		throw new NotImplementedException("Loading a world from a resource isn't implemented yet!");
	}

	public T CreateEntity<T>(bool startEnabled = true) where T : Entity, new()
	{
		var instance = new T
		{
			IsEnabled = startEnabled
		};

		AddEntity_Internal(instance);
		return instance;
	}

	public IEnumerable<Entity> GetAllEntities()
	{
		return _allEntities;
	}
	public IEnumerable<Entity> GetRootEntities()
	{
		return _rootEntities;
	}
	public IEnumerable<Entity> GetEnabledEntities()
	{
		return _enabledEntities;
	}

	public IEnumerable<Entity> GetEntitiesOfType<T>() where T : Entity
	{
		if (_typeToEntity.TryGetValue(typeof(T), out var set))
		{
			return set;
		}

		return [];
	}
	public IEnumerable<Entity> GetEntitiesWithTag<T>() where T : Tag
	{
		if (_tagToEntity.TryGetValue(typeof(T), out var set))
		{
			return set;
		}

		return [];
	}

	/// <summary>
	/// Move the specified <see cref="Entity"/> to this <see cref="World"/>.
	/// </summary>
	/// <param name="entity">The <see cref="Entity"/> to move.</param>
	public void ClaimEntity(Entity entity)
	{
		// Clean up old world.
		entity.World.RemoveEntity_Internal(entity);

		// Update new world.
		AddEntity_Internal(entity);

		// TODO: If the new world is not properly loaded (e.g. yet to be loaded) shouldn't we reset the entity to a disabled state?
	}

	internal void Load_Internal()
	{
		if (!IsLoaded)
		{
			IsLoaded = true;
			_loadedWorlds.Add(this);

			foreach (var entity in GetInitialEntities())
			{
				AddEntity_Internal(entity);
			}

			foreach (var entity in _rootEntities)
			{
				if (entity.IsEnabled)
				{
					entity.Create_Internal();
				}
			}
		}
	}

	internal static void UpdateWorlds_Internal()
	{
		foreach (var world in _loadedWorlds)
		{
			world.Update();
		}
	}

	internal static void DestroyWorlds_Internal()
	{
		foreach (var world in _loadedWorlds)
		{
			world.Destroy();
		}
	}

	internal void AddEntity_Internal(Entity entity)
	{
		_allEntities.Add(entity);

		if (entity.IsRoot)
		{
			_rootEntities.Add(entity);
		}

		if (entity.IsEnabled)
		{
			_enabledEntities.Add(entity);
		}

		if (_typeToEntity.TryGetValue(entity.GetType(), out var set))
		{
			set.Add(entity);
		}
		else
		{
			_typeToEntity.Add(entity.GetType(), [entity]);
		}

		entity.SetWorld_Internal(this);
		AssignToTagBatches(entity);
	}

	internal void RemoveEntity_Internal(Entity entity)
	{
		_allEntities.Remove(entity);
		_enabledEntities.Remove(entity);
		_rootEntities.Remove(entity);

		if (_typeToEntity.TryGetValue(entity.GetType(), out var set))
		{
			set.Remove(entity);
		}

		UnassignFromTagBatches(entity);
	}

	internal void EnableEntity_Internal(Entity entity)
	{
		if (_allEntities.Contains(entity))
		{
			_enabledEntities.Add(entity);
		}
	}

	internal void DisableEntity_Internal(Entity entity)
	{
		_enabledEntities.Remove(entity);
	}

	internal void AddEntityToTagSet_Internal(Type tag, Entity entity)
	{
		if (_tagToEntity.TryGetValue(tag, out var set))
		{
			set.Add(entity);
		}
		else
		{
			_tagToEntity.Add(tag, [entity]);
		}
	}

	internal void RemoveEntityFromTagSet_Internal(Type tag, Entity entity)
	{
		if (_tagToEntity.TryGetValue(tag, out var set))
		{
			set.Remove(entity);
		}
	}

	private void AssignToTagBatches(Entity entity)
	{
		foreach (var tag in entity.Tags)
		{
			AddEntityToTagSet_Internal(tag, entity);
		}
	}

	private void UnassignFromTagBatches(Entity entity)
	{
		foreach (var tag in entity.Tags)
		{
			RemoveEntityFromTagSet_Internal(tag, entity);
		}
	}

	internal void DelayDestroyEntity_Internal(Entity entity)
	{
		_toDestroy.Add(entity);
	}

	protected virtual IEnumerable<Entity> GetInitialEntities() => [];

	private void Update()
	{
		foreach (var entity in _rootEntities)
		{
			if (entity.IsEnabled)
			{
				entity.Update_Internal();
			}
		}

		// Remove all entities that were destroyed during the above updating.
		foreach (var entity in _toDestroy)
		{
			RemoveEntity_Internal(entity);
		}

		_toDestroy.Clear();
	}

	private void Destroy()
	{
		foreach (var entity in _allEntities)
		{
			entity.Destroy_Internal();
		}

		_allEntities.Clear();
		_enabledEntities.Clear();

		_typeToEntity.Clear();
		_tagToEntity.Clear();
	}
}
