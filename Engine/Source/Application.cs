using Engine.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

public static class Application
{
	/// <summary>
	/// Called after initializing, but before entering the main loop.
	/// </summary>
	public static event Action OnLoad;
	public static event Action OnUpdate;
	public static event Action OnQuit;

	public static void Run()
	{
		GlobalManager.Register<SpriteManager>();

		Window.Launch();
		Renderer.Initialize();
		GlobalManager.Begin();

		OnLoad?.Invoke();

		Input.Initialize();

		while (!Window.IsClosing)
		{
			Update();
		}

		OnQuit?.Invoke();

		GlobalManager.End();
		Window.CleanUp();
	}

	private static void Update()
	{
		Window.PollEvents();

		OnUpdate?.Invoke();
		GlobalManager.Update();
		World.Current?.Update();

		Renderer.Render();
		Window.SwapBuffers();

		// Reset input state for next frame.
		Input.Clear();

		// Update the time class with the current GLFW window time. Updating occurs after the main loop so that game logic starts with a time of 0s.
		double glfwTime = GLFW.GetTime();
		Time.Update((float)glfwTime);
	}

	public static void Quit()
	{
		Window.Close();
	}
}
