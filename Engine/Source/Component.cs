namespace Engine;

public abstract class Component
{
	internal void Create()
	{
		OnCreate();
	}

	internal void Update()
	{
		OnUpdate();
	}

	internal void Render()
	{
		OnRender();
	}

	internal void Destroy()
	{
		OnDestory();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnRender() { }
	protected virtual void OnDestory() { }
}
