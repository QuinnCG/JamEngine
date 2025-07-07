using Engine.Rendering;
using Engine.InputSystem;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

public static class Application
{
	/// <summary>
	/// Called after initializing, but before entering the main loop.
	/// </summary>
	public static event Action OnLoad;
	/// <summary>
	/// Called every frame, before updating global managers and entites.
	/// </summary>
	public static event Action OnUpdate;
	/// <summary>
	/// Called just before shutting down.
	/// </summary>
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
		// Reset input state for next frame.
		Input.Clear();
		// Poll new inputs.
		Window.PollEvents();

		OnUpdate?.Invoke();
		GlobalManager.Update();
		World.Current?.Update();

		Renderer.Render();
		Window.SwapBuffers();

		// Update the time class with the current GLFW window time. Updating occurs after the main loop so that game logic starts with a time of 0s.
		double glfwTime = GLFW.GetTime();
		Time.Update((float)glfwTime);
	}

	public static void Quit()
	{
		Window.Close();
	}
}
