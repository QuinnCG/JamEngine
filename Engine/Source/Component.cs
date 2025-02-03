namespace Engine;

public abstract class Component
{
	public bool DoesUpdate { get; set; } = true;

	internal void Create()
	{
		OnCreate();
	}

	internal void Update()
	{
		if (DoesUpdate)
		{
			OnUpdate();
		}
	}

	internal void Destroy()
	{
		OnDestroy();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }
}
