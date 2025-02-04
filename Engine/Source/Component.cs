namespace Engine;

public abstract class Component
{
	public Entity Entity => _entity!;
	public bool DoesUpdate
	{
		get => _doesUpdate && Entity.IsEnabled;
		set => _doesUpdate = value;
	}
	public bool IsDestroyed { get; private set; }

	protected Wait Wait { get; } = new();

	private Entity? _entity;
	private bool _doesUpdate = true;

	public T GetComponent<T>() where T : Component
	{
		return Entity.GetComponent<T>();
	}

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

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }
}
