using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Engine;

public abstract class Entity : IEnumerable<Component>
{
	protected World World => _world!;
	protected Wait Wait { get; } = new();

	private World? _world;
	private readonly Dictionary<Type, Component> _components = [];

	public IEnumerator<Component> GetEnumerator()
	{
		return _components.Values.GetEnumerator();
	}

	/// <summary>
	/// Use this to set the <c>World</c> of the <c>Entity</c> after it's been created; e.g. when being transfered to another world.
	/// <br>Note: this will remove the entity from its old world.</br>
	/// <br>Note: this does not add the entity to the specified world. As such this is supposed to be called directly by a <c>World</c>.</br>
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
