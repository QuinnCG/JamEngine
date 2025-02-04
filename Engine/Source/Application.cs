using Engine.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

	internal static bool IsEndOfFrame { get; private set; }

	public static void Run(World? world = null)
	{
		// Initialization
		Window.Launch();
		Input.Initialize();
		Renderer.Initialize();

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
			IsEndOfFrame = false;

			// Update input.
			Input.Clear();
			Window.PollEvents();

			// Update global entities.
			foreach (var entity in _globalEntities.Values)
			{
				entity.Update_Internal();
			}

			// Update game worlds.
			World.UpdateWorlds_Internal();

			// End of frame/late update.
			IsEndOfFrame = true;

			// Draw frame.
			Renderer.Render();
			Window.SwapBuffers();

			// Update late. Otherwise, not user code will ever run with time equalling 0.
			Time.Update((float)GLFW.GetTime());
		}

		// Cleanup
		foreach (var entity in _globalEntities.Values)
		{
			entity.Destroy_Internal();
		}

		World.DestroyWorlds_Internal();
		Renderer.CleanUp();
		Window.CleanUp();
	}
}
