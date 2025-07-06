using OpenTK.Mathematics;

namespace Engine;

public class Entity
{
	public World World { get; private set; }

	public Vector2 Position { get; set; }
	public float Rotation { get; set; }
	public Vector2 Scale { get; set; } = Vector2.One;

	private readonly Dictionary<Type, Component> _components = [];

	/// <summary>
	/// Sets only the x component of <see cref="Position"/>.
	/// </summary>
	public void SetX(float x)
	{
		Position = new Vector2(x, Position.Y);
	}
	/// <summary>
	/// Sets only the y component of <see cref="Position"/>.
	/// </summary>
	public void SetY(float y)
	{
		Position = new Vector2(Position.X, y);
	}

	/// <summary>
	/// Sets only the x component of <see cref="Scale"/>.
	/// </summary>
	public void SetWidth(float xScale)
	{
		Scale = new Vector2(xScale, Position.Y);
	}
	/// <summary>
	/// Sets only the y component of <see cref="Scale"/>.
	/// </summary>
	public void SetHeight(float yScale)
	{
		Scale = new Vector2(Position.X, yScale);
	}

	public T CreateComponent<T>() where T : Component, new()
	{
		if (_components.ContainsKey(typeof(T)))
		{
			throw new InvalidOperationException($"Component of type {typeof(T).Name} already exists on this entity!");
		}

		var comp = new T();
		_components.Add(typeof(T), comp);

		if (World == null || !World.IsLoaded)
		{
			comp.Create(this);
		}

		return comp;
	}

	public void DestroyComponent<T>() where T : Component
	{
		if (_components.TryGetValue(typeof(T), out var component))
		{
			if (World == null || !World.IsLoaded)
			{
				component.Destroy();
			}

			_components.Remove(typeof(T));
		}
		else
		{
			throw new InvalidOperationException($"Component of type {typeof(T).Name} does not exist on this entity!");
		}
	}

	internal void Create(World world)
	{
		World = world;

		OnCreate();

		foreach (var component in _components.Values)
		{
			component.Create(this);
		}
	}

	internal void Update()
	{
		OnUpdate();

		foreach (var component in _components.Values)
		{
			component.Update();
		}
	}

	/// <summary>
	/// Sets the internal state to destroyed. Does not remove this entity from its world.
	/// </summary>
	internal void Destroy()
	{
		OnDestroy();

		foreach (var component in _components.Values)
		{
			component.Destroy();
		}
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }
}
