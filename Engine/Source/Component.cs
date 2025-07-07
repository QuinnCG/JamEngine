using OpenTK.Mathematics;

namespace Engine;

public abstract class Component
{
	public Entity Entity { get; private set; }
	public World World => Entity.World;

	public Vector2 Position {get => Entity.Position; set => Entity.Position = value; }
	public float Rotation { get => Entity.Rotation; set => Entity.Rotation = value; }
	public Vector2 Scale { get => Entity.Scale; set => Entity.Scale = value; }

	public bool HasComponent<T>() where T : Component
	{
		return Entity.HasComponent<T>();
	}

	public T GetComponent<T>() where T : Component
	{
		return Entity.GetComponent<T>();
	}

	public bool TryGetComponent<T>(out T component) where T : Component
	{
		return Entity.TryGetComponent(out component);
	}


	internal void Create(Entity entity)
	{
		Entity = entity;
		OnCreate();
	}

	internal void Start()
	{
		OnStart();
	}

	internal void Update()
	{
		OnUpdate();
	}

	internal void LateUpdate()
	{
		OnLateUpdate();
	}

	internal void Destroy()
	{
		OnDestroy();
	}

	/// <inheritdoc cref="Entity.OnCreate"/>
	protected virtual void OnCreate() { }
	/// <inheritdoc cref="Entity.OnStart"/>
	protected virtual void OnStart() { }
	/// <inheritdoc cref="Entity.OnUpdate"/>
	protected virtual void OnUpdate() { }
	/// <inheritdoc cref="Entity.OnLateUpdate"/>
	protected virtual void OnLateUpdate() { }
	/// <inheritdoc cref="Entity.OnDestroy"/>
	protected virtual void OnDestroy() { }
}
