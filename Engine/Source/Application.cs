namespace Engine;

public static class Application
{
	private static readonly Dictionary<Type, GlobalEntity> _globalEntities = [];

	public static void RegisterGlobal<T>() where T : GlobalEntity, new()
	{
		_globalEntities.Add(typeof(T), new T());
	}

	public static void UnregisterGlobal<T>() where T : GlobalEntity
	{
		_globalEntities.Remove(typeof(T));
	}

	public static void Run(World? world = null)
	{
		// Initialization
		Window.Launch();

		if (world != null)
		{
			World.Load(world);
		}

		foreach (var entity in _globalEntities.Values)
		{
			entity.Create_Internal();
		}

		// Game Loop
		while (!Window.IsClosing)
		{
			foreach (var entity in _globalEntities.Values)
			{
				entity.Update_Internal();
			}

			World.UpdateWorlds_Internal();
			Window.Update();
		}

		// Cleanup
		foreach (var entity in _globalEntities.Values)
		{
			entity.Destroy_Internal();
		}

		World.DestroyWorlds_Internal();
		Window.CleanUp();
	}
}
