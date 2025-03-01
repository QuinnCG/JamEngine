namespace Engine;

// TODO: [World.cs] World updates root entities, which update sub entities.

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
		}

		throw new NotImplementedException();
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
		}

		throw new NotImplementedException();
	}

	/// <summary>
	/// Register an entity as being apart of this <see cref="World"/>.
	/// </summary>
	internal void AddEntity_Internal<T>(T entity) where T : Entity
	{
		// Add to root set.
		// Look for IRenderables and add to render set.
		// Cache tags, etc.

		throw new NotImplementedException();
	}

	internal void RemoveEntity_Internal<T>(T entity) where T : Entity
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Remove the specified entity from its existing <see cref="World"/> and add it to this <see cref="World"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="entity"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void TakeEntity<T>(T entity) where T : Entity
	{
		throw new NotImplementedException();
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
