namespace Engine;

public abstract class Component
{
	public Entity Entity => _entity!;
	public bool DoesUpdate { get; set; } = true;
	public bool IsDestroyed { get; private set; }

	protected Wait Wait { get; } = new();

	private Entity? _entity;

	internal void SetEntity_Internal(Entity? entity)
	{
		_entity = entity;
	}

	internal void Create_Internal()
	{
		OnCreate();
	}

	internal void Update_Internal()
	{
		if (DoesUpdate)
		{
			OnUpdate();
		}
	}

	internal void Destroy_Internal()
	{
		if (!IsDestroyed)
		{
			Wait.Cancel();
			OnDestroy();
			IsDestroyed = true;
		}
	}

	public T GetComponent<T>() where T : Component
	{
		return Entity.GetComponent<T>();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }
}
