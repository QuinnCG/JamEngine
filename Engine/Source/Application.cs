using Engine.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

public static class Application
{
	/// <summary>
	/// Has <c>Application.Launch()</c> been called yet?
	/// </summary>
	public static bool IsLaunched { get; private set; }
	/// <summary>
	/// You can change this before launching the application or after launching via <c>Time.FixedStepDivision</c>.
	/// </summary>
	public static int DefaultFixedStepDivision { get; set; } = 60;
	/// <summary>
	/// Contains <c>Window</c> options that can't be set after launching.
	/// </summary>
	public static WindowLaunchOptions WindowLaunchOptions { get; set; } = new();

	/// <summary>
	/// Called right after the <c>Application</c> is launched but just before entering the update loop.
	/// </summary>
	public static event Action? OnLaunch;
	/// <summary>
	/// Called right after <c>World.Update()</c> is called.
	/// </summary>
	public static event Action? OnUpdate;

	public static void Launch()
	{
		Time.FixedStepDivision = DefaultFixedStepDivision;

		Window.Launch(WindowLaunchOptions);
		Renderer.Initialize();

		Resource.Initialize();

		IsLaunched = true;
		OnLaunch?.Invoke();

		World.Loaded.Load();
		float nextFixedUpdate = 0f;

		while (!Window.ShouldClose)
		{
			float time = (float)GLFW.GetTime();
			Time.Update(time);

			Window.PollEvents();

			OnUpdate?.Invoke();
			World.Loaded.Update();
			
			if (Time.Now > nextFixedUpdate)
			{
				World.Loaded.FixedUpdate();
				nextFixedUpdate = Time.Now + Time.FixedDelta;
			}

			Renderer.Clear();
			Renderer.Render();

			Window.SwapBuffers();
		}

		Renderer.CleanUp();
		Window.CleanUp();
	}

	public static void Quit()
	{
		Window.Close();
	}
}
