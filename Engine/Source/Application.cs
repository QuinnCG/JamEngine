using Engine.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.InteropServices;

namespace Engine;

public static unsafe class Application
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
	public static bool CapFrameRate { get; set; } = false;
	public static int CappedFrameRate { get; set; } = 60;

	/// <summary>
	/// Called right after the <c>Application</c> is launched but just before entering the update loop.
	/// </summary>
	public static event Action? OnLaunch;
	/// <summary>
	/// Called right after <c>World.Update()</c> is called.
	/// </summary>
	public static event Action? OnUpdate;

	//[DllImport("example_glfw_opengl3.dll")]
	//private static extern void InitImGUI_Internal(OpenTK.Windowing.GraphicsLibraryFramework.Window* glfwWindow);

	//[DllImport("example_glfw_opengl3.dll")]
	//private static extern void Render_Internal();

	public static void Launch()
	{
		Time.FixedStepDivision = DefaultFixedStepDivision;

		Resource.Initialize();

		Window.Launch(WindowLaunchOptions);
		Renderer.Initialize();

		//unsafe
		//{
		//	InitImGUI_Internal(Window.Handle);
		//	//ImGui_ImplGlfw_InitForOpenGL((IntPtr)Window.Handle, true);
		//}

		//_imContext = ImGui.CreateContext();
		//ImGui.SetCurrentContext(_imContext);

		Input.Initialize();

		/* CLIENT CODE MAY EXECUTE AFTER THIS POINT */
		 
		IsLaunched = true;
		OnLaunch?.Invoke();

		World.Loaded.Load();

		float nextFixedUpdate = 0f;
		float nextRenderUpdate = 0f;

		while (!Window.ShouldClose)
		{
			float time = (float)GLFW.GetTime();
			Time.Update(time);

			Window.PollEvents();

			OnUpdate?.Invoke();
			World.Loaded.Update();
			
			if (Time.Now >= nextFixedUpdate)
			{
				World.Loaded.FixedUpdate();
				nextFixedUpdate = Time.Now + Time.FixedDelta;
			}

			if (!CapFrameRate || Time.Now >= nextRenderUpdate)
			{
				nextRenderUpdate = Time.Now + (1f / CappedFrameRate);
				Renderer.Render();

				//Render_Internal();

				//ImGuiImplOpenGL3

				//ImGui.ImGuiImplOpenGL3NewFrame();
				//ImGui.ImGuiImplGlfwNewFrame();
				//ImGui.NewFrame();

				//bool show = true;
				//ImGui.ShowDemoWindow(ref show);

				//ImGui.Render();
				//ImGui.ImGuiImplOpenGL3RenderDrawData(ImGui.GetDrawData());
			}

			Window.SwapBuffers();
			Input.Reset();
		}

		//ImGui.ImGuiImplOpenGL3Shutdown();
		//ImGui.ImGuiImplGlfwShutdown();
		//ImGui.DestroyContext(null);

		Renderer.CleanUp();
		Window.CleanUp();
	}

	public static void Quit()
	{
		Window.Close();
	}
}
