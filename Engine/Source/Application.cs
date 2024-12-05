using Engine.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

public static class Application
{
	public static bool IsLaunched { get; private set; }
	/// <summary>
	/// You can change this before launching the application or after launching via <c>Time.FixedStepDivision</c>.
	/// </summary>
	public static int DefaultFixedStepDivision { get; set; } = 60;

	public static void Launch()
	{
		Time.FixedStepDivision = DefaultFixedStepDivision;

		Window.Launch();
		Renderer.Initialize();

		IsLaunched = true;
		World.Loaded.Load();

		float nextFixedUpdate = 0f;

		while (!Window.ShouldClose)
		{
			float time = (float)GLFW.GetTime();
			Time.Update(time);

			Window.PollEvents();
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
