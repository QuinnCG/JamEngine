using Engine;

namespace Sandobx;

static class Program
{
	private static void Main()
	{
		World.SetActive(new MyWorld());

		Window.Title += " - Sandbox";
		Application.Launch();
	}
}

class MyWorld : World
{
	public override IEnumerable<Entity> OnLoad()
	{
		return [new Logger("Hello World")];
	}
}

class Logger(string message) : Entity
{
	private readonly string _msg = message;

	protected override void OnUpdate()
	{
		Log.Info(_msg);
	}
}
