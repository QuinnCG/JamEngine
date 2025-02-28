namespace Engine;

public static class Application
{
	public static bool InEditMode { get; private set; }
	public static bool InPlayMode => !InEditMode;

	/// <summary>
	/// The configuration used for this build.<br/>
	/// This can be <see cref="BuildType.Editor"/>, <see cref="BuildType.Development"/>, or <see cref="BuildType.Release"/>.
	/// </summary>
	/// <remarks>This shouldn't be confused with <see cref="InPlayMode"/> and <see cref="InEditMode"/>.</remarks>
#if EDITOR
	public static BuildType BuildType { get; } = BuildType.Editor;
#elif DEVELOPMENT
	public static BuildType BuildType { get; } = BuildType.Development;
#elif RELEASE
	public static BuildType BuildType { get; } = BuildType.Release;
#endif

	public static void Run()
	{
		Window.Launch();
		Input.Initialize();

		while (!Window.IsExited)
		{
			Input.Clear();
			Window.PollEvents();

			// App logic.
			Update();

			// Game logic.
			// TODO: [Application.cs] Update game logic and render.

			Window.SwapBuffers();
		}

		Window.CleanUp();
	}

	public static void SwitchToEditMode()
	{
		if (!InEditMode)
		{
			Log.Info("Switched to Edit Mode");

			InEditMode = true;
			Window.UpdateTitleEditorMode();
		}
	}

	public static void SwitchToPlayMode()
	{
		if (InEditMode)
		{
			Log.Info("Switched to Play Mode");

			InEditMode = false;
			Window.UpdateTitleEditorMode();
		}
	}

	/// <summary>
	/// Calls <see cref="SwitchToEditMode"/> or <see cref="SwitchToPlayMode"/> depending on the current mode.
	/// </summary>
	public static void ToggleEditMode()
	{
		if (InEditMode)
			SwitchToPlayMode();
		else
			SwitchToEditMode();
	}

	private static void Update()
	{
#if EDITOR
		// Toggle edit-mode.
		if (Input.IsKeyPressed(Key.F5))
		{
			ToggleEditMode();
		}

		// Close window.
		if (Input.IsKeyPressed(Key.Escape))
		{
			Window.Close();
		}
#endif
	}
}
