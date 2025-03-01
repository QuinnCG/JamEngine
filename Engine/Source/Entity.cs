namespace Engine;

public class Entity
{
	/// <summary>
	/// If this is false, <see cref="OnUpdate"/> will not be called for this entity or any of its children.<br/>
	/// Components of this entity won't be updated either.
	/// </summary>
	/// 
	/// <remarks>
	/// The root entities of a world still receive update calls, even if this is false.<br/>
	/// The update calls will simply be ignored.
	/// </remarks>
	public bool DoesUpdate { get; set; } = true;

	internal void Create_Internal()
	{
		throw new NotImplementedException();
	}

	internal void Update_Internal()
	{
		throw new NotImplementedException();
	}

	internal void Destroy_Internal()
	{
		throw new NotImplementedException();
	}

	protected virtual void OnCreate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnDestroy() { }

	public T GetInterfacesOfType<T>(Type type)
	{
		// Get interface on this, on components, and recursively on children.
		throw new NotImplementedException();
	}
}
