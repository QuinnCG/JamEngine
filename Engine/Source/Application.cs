using Engine.Rendering;
using nkast.Aether.Physics2D.Dynamics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

public static class Application
{
	/// <summary>
	/// Called after certain core systems are initialized, but before the first entities are loaded.
	/// </summary>
	public static event Action? OnPreLoad;
	/// <summary>
	/// Called just before entering the game-loop, but after most things are initialized.
	/// </summary>
	public static event Action? OnLoad;

	private static readonly Dictionary<Type, GlobalEntity> _globalEntities = [];

	public static void RegisterGlobal<T>() where T : GlobalEntity, new()
	{
		_globalEntities.Add(typeof(T), new T());
	}

	public static void UnregisterGlobal<T>() where T : GlobalEntity
	{
		_globalEntities.Remove(typeof(T));
	}

	/// <summary>
	/// Primarily, used by <see cref="Wait.EndOfFrame"/>.
	/// </summary>
	internal static bool IsEndOfFrame { get; private set; }

	public static void Run(World? world = null)
	{
		InitCore();
		InitEntities(world);
		EnterGameLoop();
		CleanUp();
	}

	private static void InitCore()
	{
		Window.Launch();
		Input.Initialize();
		Renderer.Initialize();
	}

	private static void InitEntities(World? world)
	{
		OnPreLoad?.Invoke();

		if (world != null)
		{
			World.Load(world);
		}

		foreach (var entity in _globalEntities.Values)
		{
			entity.Create_Internal();
		}
	}

	private static void EnterGameLoop()
	{
		OnLoad?.Invoke();

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
			Renderer.Flush();
			Window.SwapBuffers();

			// Update late. Otherwise, not user code will ever run with time equalling 0.
			Time.Update((float)GLFW.GetTime());
		}
	}

	private static void CleanUp()
	{
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
