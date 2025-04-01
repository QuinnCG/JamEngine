using Engine.Rendering;

namespace Engine;

public static class Application
{
	public static void Run()
	{
		GlobalManager.Register<SpriteManager>();

		Window.Launch();
		GlobalManager.Begin();

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
