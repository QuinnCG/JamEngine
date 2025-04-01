namespace Engine;

public class Entity
{
	public LifecycleState State { get; private set; } = LifecycleState.None;

	private readonly Dictionary<Type, Component> _components = [];

	internal void Create()
	{
		if (State is LifecycleState.None)
		{
			State = LifecycleState.Created;

			foreach (var comp in _components.Values)
			{
				comp.Create();
			}

			OnCreate();
		}
	}

	internal void Update()
	{
		if (State is LifecycleState.Created)
		{
			foreach (var comp in _components.Values)
			{
				comp.Update();
			}

			OnUpdate();
		}
	}

	internal void Destroy()
	{
		if (State is LifecycleState.Created)
		{
			State = LifecycleState.Destroyed;

			foreach (var comp in _components.Values)
			{
				comp.Destroy();
			}

			OnDestory();
		}
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestory() { }

	public bool HasComponent<T>() where T : Component
	{
		return _components.ContainsKey(typeof(T));
	}

	public T CreateComponent<T>() where T : Component, new()
	{
		var comp = new T();

		if (_components.TryAdd(typeof(T), comp))
		{
			if (State is LifecycleState.Created)
			{
				comp.Create();
			}

			return comp;
		}

		throw new Exception($"Failed to create component of type {typeof(T).Name} as one of the same type already exists on this entity.");
	}

	public bool DestroyComponent<T>() where T : Component
	{
		if (_components.TryGetValue(typeof(T), out Component? comp))
		{
			if (State is LifecycleState.Created)
			{
				comp.Destroy();
			}

			return true;
		}

		return false;
	}
}
