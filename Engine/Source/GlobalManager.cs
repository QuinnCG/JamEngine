namespace Engine;

/// <summary>
/// Register a singleton manager class that receives engine updates and isn't tied to a <see cref="World"/>.
/// </summary>
public abstract class GlobalManager
{
	private static readonly Dictionary<Type, GlobalManager> Managers = [];

	public static void Register<T>() where T : GlobalManager, new()
	{
		Managers.Add(typeof(T), new T());
	}

	public static void Unregister<T>() where T : GlobalManager
	{
		Managers.Remove(typeof(T));
	}

	public static T Get<T>() where T : GlobalManager
	{
		if (!Managers.ContainsKey(typeof(T)))
		{
			throw new Exception($"There exists no registered instance of '{typeof(T).Name}'!");
		}

		return (T)Managers[typeof(T)];
	}

	internal static void Begin()
	{
		foreach (var manager in Managers.Values)
		{
			manager.OnBegin();
		}
	}

	internal static void Update()
	{
		foreach (var manager in Managers.Values)
		{
			manager.OnUpdate();
		}
	}

	internal static void End()
	{
		foreach (var manager in Managers.Values)
		{
			manager.OnEnd();
		}
	}

	protected virtual void OnBegin() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnEnd() { }
}
