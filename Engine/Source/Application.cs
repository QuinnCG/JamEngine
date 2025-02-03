namespace Engine;

public static class Application
{
	private static readonly HashSet<GlobalEntity> _globalEntities = [];

	public static void RegisterGlobal(GlobalEntity entity)
	{
		_globalEntities.Add(entity);
	}

	public static void UnregisterGlobal(GlobalEntity entity)
	{
		_globalEntities.Remove(entity);
	}

	public static void Run()
	{
		Window.Launch();

		foreach (var entity in _globalEntities)
		{
			entity.Create_Internal();
		}

		while (!Window.IsClosing)
		{
			foreach (var entity in _globalEntities)
			{
				entity.Update_Internal();
			}

			Window.Update();
		}

		foreach (var entity in _globalEntities)
		{
			entity.Destroy_Internal();
		}

		Window.CleanUp();
	}
}
