namespace Engine;

public abstract class Component
{
	public Entity Entity => _entity!;
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

	internal void Destroy()
	{
		OnDestory();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestory() { }
}
