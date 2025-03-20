namespace Engine;

public class Entity
{
	/// <summary>
	/// If this is false, <see cref="OnUpdate"/> will not be called for this entity or any of its children.<br/>
	/// Components of this entity won't be updated either.
	/// </summary>
	/// 
	/// <remarks>
	/// The root entities of a world still receive update calls, even if this is false.<br/>
	/// The update calls will simply be ignored.
	/// </remarks>
	public bool DoesUpdate { get; set; } = true;

	public World World
	{
		get
		{
			if (_parent != null)
			{
				return _parent.World;
			}

			return World_Internal!;
		}
	}
	/// <summary>
	/// Only root entites should have worlds.<br/>
	/// Child entities will poll their parents.
	/// </summary>
	internal World? World_Internal { get; private set; }

	public Entity Parent => _parent!;
	private Entity? _parent;

	private readonly Dictionary<Type, Component> _components = [];
	private readonly HashSet<Entity> _children = [];

	/// <summary>
	/// Set the world for this entity.<br/>
	/// If this entity is a child of another entity, then it will be removed from that entity as it's now being added to a world as a root entity.
	/// </summary>
	/// <param name="world"></param>
	internal void SetWorld_Internal(World world)
	{
		World_Internal = world;

		// If you add an entity to a world directly, then it must be made a root entity, if not already.
		_parent?.RemoveChild(this);
	}

	public bool HasChild(Entity entity)
	{
		if (_children.Contains(entity))
		{
			return true;
		}

		foreach (var child in _children)
		{
			if (child.HasChild(entity))
			{
				return true;
			}
		}

		return false;
	}

	public void AddChild(Entity entity)
	{
		if (!_children.Contains(entity))
		{
			entity._parent?.RemoveChild(entity);
			_children.Add(entity);
			entity._parent = this;
		}
	}

	public void RemoveChild(Entity entity)
	{
		if (_children.Remove(entity))
		{
			if (entity._parent != null)
			{
				// This will poll upwards to the root entity.
				entity.SetWorld_Internal(entity.World);
				entity._parent = null;
			}
		}
	}

	internal void Create_Internal()
	{
		foreach (var comp in _components.Values)
		{
			comp.Create_Internal();
		}

		Create_Internal();

		foreach (var child in _children)
		{
			child.Create_Internal();
		}
	}

	internal void Update_Internal()
	{
		foreach (var comp in _components.Values)
		{
			comp.Update_Internal();
		}

		Update_Internal();

		foreach (var child in _children)
		{
			child.Update_Internal();
		}
	}

	internal void Destroy_Internal()
	{
		foreach (var child in _children)
		{
			child.Destroy_Internal();
		}

		Destroy_Internal();

		foreach (var comp in _components.Values)
		{
			comp.Destroy_Internal();
		}
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }

	public IEnumerable<T> GetInterfacesOfType<T>() where T : class
	{
		var type = typeof(T);
		Log.Assert(type.IsInterface, "Entity.GetInterfacesOfType() should only be passed the type of an interface!");

		var interfaces = new List<T>();

		if (GetType() == type)
		{
			if (this is T casted)
			{
				interfaces.Add(casted);
			}
		}

		foreach (var comp in _components.Values)
		{
			if (comp.GetType() == type)
			{
				if (comp is T casted)
				{
					interfaces.Add(casted);
				}
			}
		}

		foreach (var child in _children)
		{
			var results = child.GetInterfacesOfType<T>();
			if (results != null)
			{
				interfaces.AddRange(results);
			}
		}

		return interfaces;
	}
}
