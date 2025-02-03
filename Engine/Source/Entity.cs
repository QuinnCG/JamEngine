namespace Engine;

public class Entity
{
	public const string LogCategory = "Entity";

	// Can be created itself for dynamic entities or inherited from.
	// Supports parenting.

	public string Name
	{
		get
		{
			return GetType().Name;
		}
	}

	public Entity? Parent
	{
		get
		{
			return _parent;
		}
		set
		{
			// Setting to parent.
			if (value is not null)
			{
				// Add this to child set of value.
				bool added = value._children.Add(this);

				// Remove this from old parent's child set.
				if (added && _parent != null)
				{
					_parent._children.Remove(this);
				}
			}
			// Nulling out parent.
			else
			{
				// Remove this from old parent's child set.
				_parent?._children.Remove(this);
			}

			// Set new parent for this.
			_parent = value;
		}
	}
	public Entity Root
	{
		get
		{
			Entity entity = this;

			// Keep going up until just before reaching null.
			// If this is the top-most entity then this will be returned.
			while (entity.Parent != null)
			{
				entity = entity.Parent;
			}

			return entity;
		}
	}

	public World World
	{
		get
		{
			Log.Assert(_world != null, LogCategory, $"Failed to get world on entity '{Name}' as no world was set!");
			return _world!;
		}
	}

	public IEnumerable<Type> Tags => _tags;

	public IEnumerable<Entity> Children => _children;
	public IEnumerable<Component> Components => _components.Values;
	public IEnumerable<Type> ComponentTypes => _components.Keys;

	/// <summary>
	/// Has <see cref="OnCreate"/> been called once yet?
	/// </summary>
	public bool IsCreated { get; private set; }
	public bool IsEnabled
	{
		get => _isEnabled;
		set
		{
			if (value != _isEnabled)
			{
				_isEnabled = value;

				if (_isEnabled)
				{
					World.EnableEntity_Internal(this);

					if (!IsCreated)
					{
						Create_Internal();
					}
				}
				else
				{
					World.DisableEntity_Internal(this);
				}
			}
		}
	}
	public bool IsDestroyed { get; private set; }

	private World? _world;
	private bool _isEnabled = true;

	private readonly HashSet<Type> _tags = [];

	private Entity? _parent;
	private readonly HashSet<Entity> _children = [];

	private readonly Dictionary<Type, Component> _components = [];

	public override string ToString()
	{
		return Name;
	}

	public bool HasTag<T>() where T : Tag
	{
		return _tags.Contains(typeof(T));
	}

	public void AddTag<T>() where T : Tag
	{
		var tag = typeof(T);

		_tags.Add(tag);
		World.AddEntityToTagSet_Internal(tag, this);
	}

	public void RemoveTag<T>() where T : Tag
	{
		var tag = typeof(T);

		_tags.Remove(tag);
		World.RemoveEntityFromTagSet_Internal(tag, this);
	}

	public bool HasChild<T>(T child) where T : Entity
	{
		return _children.Contains(child);
	}

	public T AddChild<T>(T child) where T : Entity
	{
		// Add child if it isn't already a child of this entity.
		if (_children.Add(child))
		{
			// Clean up child's old parent.
			child._parent?._children.Remove(child);

			// Add child to this entity's child set.
			_children.Add(child);

			child._parent = this;
		}

		return child;
	}

	public void RemoveChild<T>(T child) where T : Entity
	{
		// Remove child only if it is a child of this entity.
		if (_children.Remove(child))
		{
			child._parent = null;
			_children.Remove(child);
		}
	}

	public T GetChild<T>(int index) where T : Entity
	{
		if (index < 0 || index >= _children.Count)
		{
			throw new ArgumentOutOfRangeException($"Failed to get child on entity '{Name}' as the index ({index}) was out of range!");
		}

		if (_children.ElementAt(index) as T is T comp)
		{
			return comp;
		}

		Log.Error(LogCategory, $"Got child at index '{index}' but failed to convert to type '{typeof(T)}' on entity '{Name}'!");
		return null;
	}

	public bool HasComponent<T>() where T : Component
	{
		return _components.ContainsKey(typeof(T));
	}

	public T GetComponent<T>() where T : Component
	{
		if (_components.TryGetValue(typeof(T), out var result) && result is T component)
		{
			return component;
		}

		Log.Error(LogCategory, $"Failed to get component of type '{typeof(T)}' on entity '{Name}'!");
		return null;
	}

	public void AddComponent<T>(T component) where T : Component
	{
		_components.Add(typeof(T), component);
	}

	public void RemoveComponent<T>() where T : Component
	{
		_components.Remove(typeof(T));
	}

	public T CreateComponent<T>() where T : Component, new()
	{
		var instance = new T();
		_components.Add(typeof(T), instance);

		return instance;
	}

	public void Destroy()
	{
		Destroy_Internal();

		World.DelayDestroyEntity_Internal(this);
		_world = null;
	}

	internal void SetWorld_Internal(World world)
	{
		_world = world;
	}

	internal void Create_Internal()
	{
		// Create components first.
		foreach (var component in _components.Values)
		{
			component.Create();
		}

		// Then create self.
		OnCreate();

		// Finally, create child entities.
		foreach (var child in _children)
		{
			if (child.IsEnabled)
			{
				child.Create_Internal();
			}
		}

		IsCreated = true;
	}

	internal void Update_Internal()
	{
		// Update components first.
		foreach (var component in _components.Values)
		{
			component.Update();
		}

		// Then update self.
		OnUpdate();

		// Finally, update child entities.
		foreach (var child in _children)
		{
			if (child.IsEnabled)
			{
				child.Update_Internal();
			}
		}
	}

	internal void Destroy_Internal()
	{
		if (!IsDestroyed && IsCreated)
		{
			// Destroy components first.
			foreach (var component in _components.Values)
			{
				component.Destroy();
			}

			// Then destroy self.
			OnDestroy();

			// Finally, destroy child entities.
			foreach (var child in _children)
			{
				child.Destroy_Internal();
			}

			_isEnabled = false;
			IsDestroyed = true;
		}
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }
}
