using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Engine;

/// <summary>
/// The abstract class for entities to enhierit from.
/// <br>An entity exists in a <see cref="Engine.World"/> and recieves <see cref="Application"/> updates.</br>
/// <br>Youy can enumerate an entity to access its child entities.</br>
/// </summary>
public abstract class Entity : IEnumerable<Entity>
{
	/// <summary>
	/// The <c>Entity</c> that is parent to this <c>Entity</c>.
	/// <br>This can be null if this <c>Entity</c> has no parent.</br>
	/// </summary>
	public Entity? Parent
	{
		get => _parent;

		set
		{
			// Ignore redundant update.
			if (_parent != value)
			{
				// If we have another parent, remove us from that parent.
				_parent?._children.Remove(this);

				// Set new parent.
				_parent = value;
				// Add to children of new parent if not setting parent to null.
				if (value != null && _parent != null)
					_parent._children.Add(this);
			}
		}
	}

	protected World World => _world!;
	protected Wait Wait { get; } = new();

	private Entity? _parent;
	private World? _world;

	private readonly Dictionary<Type, Component> _components = [];
	private readonly HashSet<Entity> _children = [];

	public bool HasChild(Entity entity)
	{
		return _children.Contains(entity);
	}

	public void AddChild(Entity entity)
	{
		_children.Add(entity);
		entity.Parent = this;
	}

	public void RemoveChild(Entity entity)
	{
		Log.Assert(HasChild(entity), $"Cannot from entity '{entity}' from parent entity '{this}' because it is not a child of that entity!");
		_children.Remove(entity);
		entity._parent = null;
	}

	public IEnumerator<Entity> GetEnumerator()
	{
		return _children.GetEnumerator();
	}

	/// <summary>
	/// Use this to set the <see cref="Engine.World"/> of the <see cref="Entity"/> after it's been created; e.g. when being transfered to another world.
	/// <br>This will remove the <see cref="Entity"/> from its old world.</br>
	/// <br>This does not add the <see cref="Entity"/> to the specified world. As such this is supposed to be called directly by a <see cref="Engine.World"/>.</br>
	/// </summary>
	internal void SetWorld(World world)
	{
		_world?.RemoveEntity(this);
		_world = world;
	}

	internal void Create(World world)
	{
#if DEBUG
		foreach (var component in _components.Values)
		{
			ValidateComponentDependency(component);
		}
#endif

		_world = world;
		OnCreate();

		foreach (var component in _components.Values)
		{
			component.Create(this);
		}
	}

	internal void Update()
	{
		foreach (var component in _components.Values)
		{
			component.Update();
		}

		OnUpdate();
	}

	internal void FixedUpdate()
	{
		foreach (var component in _components.Values)
		{
			component.FixedUpdate();
		}

		OnFixedUpdate();
	}

	internal void Destroy()
	{
		OnDestroy();

		foreach (var component in _components.Values)
		{
			component.Destroy();
		}

		Wait.Destroy();
	}

	internal T GetComponent_Internal<T>() where T : Component
	{
		return GetComponent<T>();
	}

	/// <summary>
	/// Called when the world is loaded or when the entity is created after the world has loaded.
	/// </summary>
	protected virtual void OnCreate() { }
	/// <summary>
	/// Called every frame. This updates as fast as possible.
	/// </summary>
	protected virtual void OnUpdate() { }
	/// <summary>
	/// Updates at a fixed interval. Configure this interval via <c>Application.DefaultFixedStepDivision</c> or <c>Time.FixedStepDivision</c>.
	/// </summary>
	protected virtual void OnFixedUpdate() { }
	/// <summary>
	/// The entity has been removed from its world and is no longer part of a world.
	/// <br>It is not actually "destroyed" yet; that will only happen if there are no references to it and the C# garbage collector decides to collect.</br>
	/// </summary>
	protected virtual void OnDestroy() { }

	protected bool HasComponent<T>() where T : Component
	{
		return _components.ContainsKey(typeof(T));
	}

	protected T CreateComponent<T>() where T : Component, new()
	{
		Log.Assert(!HasComponent<T>(), $"Cannot create component of type '{typeof(T)}'! There can only be one instance of any type on a given entity.");

		var comp = new T();
		CreateComponent(comp);

		return comp;
	}
	protected void CreateComponent<T>(T component) where T : Component
	{
		Log.Assert(!HasComponent<T>(), $"Cannot create component of type '{typeof(T)}'! There can only be one instance of any type on a given entity.");

		_components.Add(typeof(T), component);

		if (_world != null && World.IsLoaded)
		{
			ValidateComponentDependency(component);
			component.Create(this);
		}
	}

	protected T GetComponent<T>() where T : Component
	{
		Debug.Assert(HasComponent<T>(), $"Failed to find component '{typeof(T).Name}' on entity '{this}'!");
		return (T)_components[typeof(T)];
	}

	protected void DestroyComponent<T>(T component) where T : Component
	{
		if (HasComponent<T>())
		{
			component.Destroy();
			_components.Remove(typeof(T));
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	[Conditional("DEBUG")]
	private void ValidateComponentDependency<T>(T component) where T : Component
	{
		var attributes = component.GetType().GetCustomAttributes(typeof(DependOn<>));

		foreach (var attribute in attributes)
		{
			Type dependedOnType = attribute.GetType().GetGenericArguments()[0];
			bool found = false;

			foreach (var c in _components.Values)
			{
				if (c.GetType() == dependedOnType)
				{
					found = true;
					break;
				}
			}

			if (!found)
			{
				Log.Error($"Component '{component}' on entity '{this}' does depends on component of type '{dependedOnType}'!");
			}
		}
	}
}
