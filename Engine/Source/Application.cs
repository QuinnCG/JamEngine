namespace Engine;

public static class Application
{
	public static bool InEditMode { get; private set; }
	public static bool InPlayMode => !InEditMode;

	public static void Run()
	{
		Window.Launch();

		while (!Window.IsExited)
		{
			Window.PollEvents();
			// Game logic.
			Window.SwapBuffers();
		}

		Window.CleanUp();
	}

	public static void SwitchToEditMode()
	{
		if (!InEditMode)
		{
			InEditMode = true;
			Log.Info("Switched to Edit Mode");
		}
	}

	public static void SwitchToPlayMode()
	{
		if (InEditMode)
		{
			InEditMode = false;
			Log.Info("Switched to Play Mode");
		}
	}
}
