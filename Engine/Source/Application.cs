using Engine.Rendering;

namespace Engine;

public static class Application
{
	public static void Run()
	{
		Window.Launch();

		while (!Window.IsClosing)
		{
			Window.PollEvents();
			World.UpdateAll();
			Renderer.Render();
			Window.SwapBuffers();
		}

		Window.CleanUp();
	}
}
