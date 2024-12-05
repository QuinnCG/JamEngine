namespace Engine;

public abstract class World
{
	public static World Active => _active!;
	private static World? _active;

	public bool IsLoaded { get; private set; }

	private readonly HashSet<Entity> _entites = [];

	public static void SetActive(World world)
	{
		_active = world;
		world.Load();
	}

	public abstract IEnumerable<Entity> OnLoad();

	public void Load()
	{
		if (!IsLoaded)
		{
			IsLoaded = true;

			foreach (var entity in OnLoad())
			{
				_entites.Add(entity);
				entity.Create(this);
			}
		}
	}

	public void Unload()
	{
		if (IsLoaded)
		{
			IsLoaded = false;

			foreach (var entity in _entites)
			{
				entity.Destroy();
			}
		}
	}

	public bool ContainsEntity(Entity entity)
	{
		return _entites.Contains(entity);
	}

	public void AddEntity(Entity entity)
	{
		_entites.Add(entity);
		entity.SetWorld(this);
	}

	public void DestroyEntity(Entity entity)
	{
		Log.Assert(_entites.Contains(entity), $"Failed to destroy entity as it's not apart of this world!");
		entity.Destroy();
	}

	/// <summary>
	/// Silently remove the entity from this world.
	/// </summary>
	internal void RemoveEntity(Entity entity)
	{
		_entites.Remove(entity);
	}

	internal void Update()
	{
		foreach (var entity in _entites)
		{
			entity.Update();
		}
	}

	internal void FixedUpdate()
	{
		foreach (var entity in _entites)
		{
			entity.FixedUpdate();
		}
	}
}
