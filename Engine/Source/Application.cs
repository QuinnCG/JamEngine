using Engine.Rendering;

namespace Engine;

public static class Application
{
	/// <summary>
	/// Called after initializing, but before entering the main loop.
	/// </summary>
	public static event Action? OnLoad;

	public static void Run()
	{
		GlobalManager.Register<SpriteManager>();

		Window.Launch();
		Renderer.Initialize();
		GlobalManager.Begin();

		OnLoad?.Invoke();

		// TODO: Implement Input and Time class calls.

		while (!Window.IsClosing)
		{
			Window.PollEvents();

			GlobalManager.Update();
			World.UpdateAll();

			Renderer.Render();
			Window.SwapBuffers();
		}

		GlobalManager.End();
		Window.CleanUp();
	}
}
