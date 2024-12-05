namespace Engine;

public abstract class World
{
	public static World Loaded => _loaded!;
	private static World? _loaded;

	public bool IsLoaded { get; private set; }

	private readonly HashSet<Entity> _entites = [];
	private readonly HashSet<Entity> _toDestroy = [];

	public abstract IEnumerable<Entity> OnLoad();

	/// <summary>
	/// Unloads any existing world and loads this world.
	/// <br>Loaded worlds receive updates.</br>
	/// <br><c>Application</c> will call this during launch, but it should still be called by the client at least once before launching the app.</br>
	/// <br>Calling this before the <c>Application</c> has launched will "prime" the world to be automatically loaded after the <c>Application is launched.</c></br>
	/// <br>You can call this after <c>Application</c> has launched. It must just be called once on some world before launch.</br>
	/// </summary>
	public void Load()
	{
		_loaded?.Unload();
		_loaded = this;

		if (!Application.IsLaunched)
			return;

		if (!IsLoaded)
		{
			foreach (var entity in OnLoad())
			{
				_entites.Add(entity);
				entity.Create(this);
			}

			IsLoaded = true;
		}
	}

	public void Unload()
	{
		if (IsLoaded)
		{
			foreach (var entity in _entites)
			{
				entity.Destroy();
			}

			IsLoaded = false;
			_entites.Clear();
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
		_toDestroy.Add(entity);
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
		foreach (var entity in _toDestroy)
		{
			_entites.Remove(entity);
			entity.Destroy();
		}

		_toDestroy.Clear();

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
