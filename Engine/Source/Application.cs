namespace Engine;

public static class Application
{
	public static void Run()
	{
		Window.Launch();

		while (!Window.IsClosing)
		{
			Window.PollEvents();
			World.UpdateWorlds();
			Renderer.Render();
			Window.SwapBuffers();
		}
	}
}
