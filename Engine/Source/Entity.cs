using Engine.Source.Simulation;
using OpenTK.Mathematics;

namespace Engine;

/// <summary>
/// A thing that can exist in a <see cref="Engine.World"/>. It can have <see cref="Component"/>s attached to it, as well.<br/>
/// Entities have world-space transforms, and do not support having child or parent entities.
/// </summary>
public class Entity
{
	/// <summary>
	/// The world that this entity exists in. This is what updates this entity.
	/// </summary>
	public World World { get; private set; }

	/// <summary>
	/// The world-space position of this entity.
	/// </summary>
	public Vector2 Position
	{
		get => RawPosition;
		set
		{
			Vector2 old = RawPosition;
			RawPosition  = value;

			OnPositionChange?.Invoke(old, value);
		}
	}
	/// <summary>
	/// The world-space rotation of this entity.
	/// </summary>
	public float Rotation
	{
		get => RawRotation;
		set
		{
			float old = RawRotation;
			RawRotation = value;

			OnRotationChange?.Invoke(old, value);
		}
	}
	/// <summary>
	/// The world-space scale of this entity. This is (1, 1), by default.
	/// </summary>
	public Vector2 Scale
	{
		get => RawScale;
		set
		{
			Vector2 old = RawScale;
			RawScale = value;

			OnScaleChange?.Invoke(old, value);
		}
	}

	/// <summary>
	/// Called when the position is set.
	/// </summary>
	public event ValueChange<Vector2> OnPositionChange;
	/// <summary>
	/// Called when the rotation is set.
	/// </summary>
	public event ValueChange<float> OnRotationChange;
	/// <summary>
	/// Called when the scale is set.
	/// </summary>
	public event ValueChange<Vector2> OnScaleChange;

	/// <summary>
	/// Setting this won't call <see cref="OnPositionChange"/>.
	/// </summary>
	internal Vector2 RawPosition { get; set; }
	/// <summary>
	/// Setting this won't call <see cref="OnRotationChange"/>.
	/// </summary>
	internal float RawRotation { get; set; }
	/// <summary>
	/// Setting this won't call <see cref="OnScaleChange"/>.
	/// </summary>
	internal Vector2 RawScale { get; set; } = Vector2.One;

	// The components attached to this entity. An entity updates its components.
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

	/// <summary>
	/// Search this entity and all of its components and return the first instance of the given type.
	/// </summary>
	/// <typeparam name="T">The type to search for. This can be a component, an interface, etc.</typeparam>
	/// <returns>The found instance, or null, if it wasn't found.</returns>
	public T GetFirstType<T>()
	{
		if (this is T e)
		{
			return e;
		}
		else
		{
			foreach (var component in _components)
			{
				if (component is T c)
				{
					return c;
				}
			}
		}

		return default;
	}

	/// <summary>
	/// Get all instances of the specified type on this entity and all of its components.
	/// </summary>
	/// <typeparam name="T">The type to search for. This can be a component, an interface, etc.</typeparam>
	/// <returns>The found instances, or an empty set, if none were found.</returns>
	public IEnumerable<T> GetAllTypes<T>()
	{
		var set = new HashSet<T>();

		if (this is T e)
		{
			set.Add(e);
		}
		else
		{
			foreach (var component in _components)
			{
				if (component is T c)
				{
					set.Add(c);
				}
			}
		}

		return set;
	}

	public T GetComponent<T>() where T : Component
	{
		if (_components.TryGetValue(typeof(T), out Component c))
		{
			return c as T;
		}

		return default;
	}

	public bool HasComponent<T>()
	{
		return _components.ContainsKey(typeof(T));
	}

	public bool TryGetComponent<T>(out T component) where T : Component
	{
		bool success = _components.TryGetValue(typeof(T), out var c);

		component = c as T;
		return success;
	}

	public IEnumerable<T> GetAllComponents<T>() where T : Component
	{
		var set = new HashSet<T>();

		foreach (var comp in _components.Values)
		{
			if (comp is T c)
			{
				set.Add(c);
			}
		}

		return set;
	}

	/// <summary>
	/// Create a component and add it to this entity.
	/// </summary>
	/// <typeparam name="T">The type of the component. Must inherit from <see cref="Component"/>.<br/>An entity can only have one component per unique type of component.</typeparam>
	/// <returns>The created instance of said component.</returns>
	public T CreateComponent<T>() where T : Component, new()
	{
		if (_components.ContainsKey(typeof(T)))
		{
			throw new InvalidOperationException($"Component of type {typeof(T).Name} already exists on this entity!");
		}

		var comp = new T();
		_components.Add(typeof(T), comp);

		if (World != null && World.IsLoaded)
		{
			if (comp is IPhysicsUpdateable phys)
			{
				World.RegisterPhyiscsCallback(phys);
			}

			comp.Create(this);
			comp.Start();
		}

		return comp;
	}

	/// <summary>
	/// Add a preconstructed component. Useful, if you wish to use a component with a constructor that has parameters.
	/// </summary>
	public void AddComponent(Component component)
	{
		if (_components.ContainsKey(component.GetType()))
		{
			throw new InvalidOperationException($"Component of type {component.GetType().Name} already exists on this entity!");
		}
		else if (component.Entity != null)
		{
			throw new Exception("A component can't be attached to two entities!");
		}

		_components.Add(component.GetType(), component);

		if (World != null && World.IsLoaded)
		{
			if (component is IPhysicsUpdateable phys)
			{
				World.RegisterPhyiscsCallback(phys);
			}

			component.Create(this);
			component.Start();
		}
	}

	/// <summary>
	/// Destroy a component of a particular type.<br/>
	/// This is not deferred and will occur instantly. You shouldn't call this inside component method calls like <see cref="OnCreate"/>, <see cref="OnUpdate"/>, etc.
	/// </summary>
	/// <typeparam name="T">The type of the component to destroy.</typeparam>
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

		if (World.IsLoaded)
		{
			OnCreate();

			foreach (var comp in _components.Values)
			{
				comp.Create(this);
			}
		}
	}

	internal void Start()
	{
		foreach (var comp in _components.Values)
		{
			comp.Start();
		}

		OnStart();
	}

	internal void Update()
	{
		foreach (var comp in _components.Values)
		{
			comp.Update();
		}

		OnUpdate();
	}

	internal void LateUpdate()
	{
		foreach (var comp in _components.Values)
		{
			comp.LateUpdate();
		}

		OnLateUpdate();
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

	/// <summary>
	/// Called first. Acts like a pseudo constructor.
	/// </summary>
	protected virtual void OnCreate() { }
	/// <summary>
	/// Called after <see cref="OnCreate"/>.<br/>
	/// When a <see cref="Engine.World"/> is first loaded, all entities and components have <see cref="OnCreate"/> called before a single <see cref="OnStart"/> is called.
	/// </summary>
	protected virtual void OnStart() { }
	/// <summary>
	/// Called every-frame. Use this for common game-logic.<br/>
	/// This is not meant for rendering or physics.
	/// </summary>
	protected virtual void OnUpdate() { }
	/// <summary>
	/// Called after all <see cref="OnUpdate"/>s are called for this frame.
	/// </summary>
	protected virtual void OnLateUpdate() { }
	/// <summary>
	/// Called just before destruction.
	/// </summary>
	protected virtual void OnDestroy() { }
}
