using System.Collections;

namespace Engine;

public abstract class Entity : IEnumerable<Component>
{
	protected World World => _world!;
	protected Wait Wait { get; } = new();

	private World? _world;
	private readonly Dictionary<Type, Component> _components = [];

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
		_components.Add(typeof(T), comp);

		if (World.IsLoaded)
		{
			comp.Create(this);
		}

		return comp;
	}

	protected T GetComponent<T>() where T : Component
	{
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

	public IEnumerator<Component> GetEnumerator()
	{
		return _components.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
