using Engine;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		new MyWorld().Load();
		Application.OnUpdate += OnUpdate;

		Window.Title += " - Sandbox";
		Application.Launch();
	}

	private static void OnUpdate()
	{
		if (Input.IsPressed(Key.Escape))
		{
			Application.Quit();
		}
	}
}
