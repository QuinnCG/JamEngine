namespace Engine;

public abstract class World
{
	public static World Loaded => _loaded!;
	private static World? _loaded;

	/// <summary>
	/// Is loaded and active. Only one <see cref="World"/> can be active at a time.
	/// </summary>
	public bool IsLoaded { get; private set; }

	private readonly HashSet<Entity> _entites = [];
	private readonly HashSet<Entity> _toAdd = [];
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
			// In case any entities were added outside of OnLoad().
			foreach (var entity in _entites)
			{
				entity.Create(this);
			}

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

	public bool HasEntity(Entity entity)
	{
		return _entites.Contains(entity);
	}

	/// <summary>
	/// Remove this <see cref="Entity"/> from its old <see cref="World"/> (if it had one) and add it to this one.
	/// <br>Adding is deferred until the start of the next update.</br>
	/// </summary>
	public void AddEntity(Entity entity)
	{
		if (!HasEntity(entity))
		{
			_toAdd.Add(entity);
		}
	}

	/// <summary>
	/// Note: adding is deferred until the start of the next update.
	/// </summary>
	public void DestroyEntity(Entity entity)
	{
		Log.Assert(_entites.Contains(entity), $"Failed to destroy entity as it's not apart of this world or was and has already been destroyed!");
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
		foreach (var entity in _toAdd)
		{
			entity.SetWorld(this);
			_entites.Add(entity);
		}

		_toAdd.Clear();


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
