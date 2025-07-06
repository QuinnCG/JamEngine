namespace Engine;

public abstract class Component
{
	public Entity Entity { get; private set; }

	internal void Create(Entity entity)
	{
		Entity = entity;
		OnCreate();
	}

	internal void Update()
	{
		OnUpdate();
	}

	internal void Destroy()
	{
		OnDestroy();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }
}
