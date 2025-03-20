using Engine.Rendering;

namespace Engine;

// TODO: [World.cs] World updates root entities, which update sub entities.
// TODO: [World.cs] Editor world is loaded like a normal world and updated like one too. It is unloaded when switching to playmode.

public class World
{
	// Worlds that have been started.
	private static readonly HashSet<World> _runningWorlds = [];

	/// <summary>
	/// The world has been constructed from a world resource.<br/>
	/// Any entities of that resource have been added to this world.<br/>
	/// The world has not been started yet.<br/><br/>
	/// 
	/// The world is allowed to start while this is false.<br/>
	/// This should only be true if <see cref="Load(string)"/> was called to deserialize the world from a resource.
	/// </summary>
	public bool IsLoaded { get; private set; }
	/// <summary>
	/// The world is running. All entities have been created. Any new entities will automatically be created upon being added.<br/>
	/// The world is receiving updates and propgating them to its entities, where applicable.
	/// </summary>
	public bool IsStarted { get; private set; }
	public bool IsDestroyed { get; private set; }

	// The entities without a parent entity.
	// These are updated and in turn update their children.
	private readonly HashSet<Entity> _rootEntities = [];
	// All the IRenderables found.
	private readonly HashSet<IRenderable> _renderables = [];

	/// <summary>
	/// Construct the world from a world asset resource.<br/>
	/// This does not begin the world; for that use <see cref="Start"/>.
	/// </summary>
	/// 
	/// <remarks>
	/// You can call this multiple times from different world resources, to deserialize and construct the entities from them.<br/>
	/// The new entities will simply be added without replacing the existing entities.,
	/// </remarks>
	public void Load(string path)
	{
		// TODO: Use resource instead of string path.
		throw new NotImplementedException();
	}

	/// <summary>
	/// Make the world be in its started-state and trigger <c>OnCreate</c> for all of its entities.<br/><br/>
	/// 
	/// How ever the world is constructed, you will need to start it at some point.<br/>
	/// Starting it will trigger <c>OnCreate()</c> for all the entities in the world.<br/>
	/// It will also allow the world to begin receiving updates.
	/// </summary>
	/// 
	/// <remarks>
	/// You don't have to call <see cref="Load(string)"/> before calling this.<br/>
	/// In fact, you can call this on an empty <see cref="World"/>, then add entities to it later.<br/>
	/// You may also directly add entities to the <see cref="World"/> before calling this; no need for deserializing a world resource.
	/// </remarks>
	public void Start()
	{
		if (!IsStarted)
		{
			IsStarted = true;
			_runningWorlds.Add(this);

			// Update any entities that exist so far.
			foreach (var entity in _rootEntities)
			{
				entity.Create_Internal();
			}
		}
	}

	/// <summary>
	/// Checks if this world's root entities or any of their child entities are the specified entity.
	/// </summary>
	/// <param name="entity">The specified entity you are looking for.</param>
	/// <returns>True, if the world contains the specified entity.</returns>
	public bool ContainsEntity(Entity entity)
	{
		if (_rootEntities.Contains(entity))
		{
			return true;
		}

		return entity.HasChild(entity);
	}

	/// <summary>
	/// Update the world.<br/>
	/// This will update all root enitites, which in turn update their hierarchies.<br/>
	/// This will also update all renderables.
	/// </summary>
	internal void Update_Internal()
	{
		foreach (var entity in _rootEntities)
		{
			entity.Update_Internal();
		}

		// TODO: Render.
		// Consider render time.
		// Consider renderer class that handles rendering stuff.
	}

	/// <summary>
	/// Destroy all entities in this world and unstart it.
	/// </summary>
	public void Destroy()
	{
		if (!IsDestroyed)
		{
			IsStarted = false;
			IsLoaded = false;

			IsDestroyed = true;
			_runningWorlds.Remove(this);

			foreach (var entity in _rootEntities)
			{
				entity.Destroy_Internal();
			}
		}

		throw new NotImplementedException();
	}

	/// <summary>
	/// Register an entity as being apart of this <see cref="World"/>.
	/// </summary>
	internal void AddEntity_Internal<T>(T entity) where T : Entity
	{
		// TODO: Cache tags and types for fast lookup.

		// Only refuse if trying to add root entity.
		// If entity is not root entity but still apart of world, we can just move them to root and so this method is still valid to call.
		if (!_rootEntities.Contains(entity))
		{
			_rootEntities.Add(entity);
			entity.SetWorld_Internal(this);

			var renderables = entity.GetInterfacesOfType<IRenderable>();
			foreach (var renderable in renderables)
			{
				_renderables.Add(renderable);
			}

			// If world is started, then call create.
			if (IsStarted)
			{
				entity.Create_Internal();
			}
		}
	}

	internal void RemoveEntity_Internal<T>(T entity) where T : Entity
	{
		if (_rootEntities.Remove(entity))
		{
			var renderables = entity.GetInterfacesOfType<IRenderable>();
			foreach (var renderable in renderables)
			{
				_renderables.Remove(renderable);
			}

			if (IsStarted)
			{
				entity.Destroy_Internal();
			}
		}
	}

	/// <summary>
	/// Remove the specified entity from its existing <see cref="World"/> and add it to this <see cref="World"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="entity"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void TakeEntity<T>(T entity) where T : Entity
	{
		if (!ContainsEntity(entity))
		{
			entity.World_Internal?.RemoveEntity_Internal(entity);
			AddEntity_Internal(entity);
		}
	}

	/// <summary>
	/// Create an instance of the specified entity and add it to this world.
	/// </summary>
	/// <typeparam name="T">The entity type to create.</typeparam>
	/// <returns>A reference to the created instance.</returns>
	public T CreateEntity<T>() where T : Entity, new()
	{
		var entity = new T();
		AddEntity_Internal(entity);
		return entity;
	}
}
