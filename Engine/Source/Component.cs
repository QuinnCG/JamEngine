namespace Engine;

public abstract class Component
{
	protected Entity Entity => _entity!;

	private Entity? _entity;

	internal void Create(Entity entity)
	{
		_entity = entity;
		OnCreate();
	}

	internal void Update()
	{
		OnUpdate();
	}

	internal void FixedUpdate()
	{
		OnFixedUpdate();
	}

	internal void Destroy()
	{
		OnDestroy();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnFixedUpdate() { }
	protected virtual void OnDestroy() { }
}
