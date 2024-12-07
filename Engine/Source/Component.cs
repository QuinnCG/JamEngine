namespace Engine;

public abstract class Component
{
	protected Entity Entity => _entity!;
	protected Wait Wait { get; } = new();

	private Entity? _entity;

	internal void Create(Entity entity)
	{
		Log.Assert(_entity == null, $"Cannot attach component '{this}' to entity '{entity}' because it is already attached to entity '{_entity}'!");

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
		Wait.Destroy();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnFixedUpdate() { }
	protected virtual void OnDestroy() { }

	/// <summary>
	/// This can't be called before the parent <c>Entity</c> has been initialized.
	/// <br>Either call this afterwards (such as in <c>OnCreate</c>) or make sure this component is only ever created on an already active <c>Entity</c>.</br>
	/// </summary>
	/// <typeparam name="T">The type of the <c>Component</c> to get.</typeparam>
	/// <returns>The retrieved component. An assertion is used to make sure the component exists.
	/// <br>If you can't be certain the <c>Component</c> will exist then make sure to use <c>Entity.HasComponent()</c> first.</br></returns>
	protected T GetComponent<T>() where T : Component
	{
		return Entity.GetComponent_Internal<T>();
	}
}
