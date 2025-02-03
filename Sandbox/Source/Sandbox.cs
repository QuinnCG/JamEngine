using Engine;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		Application.RegisterGlobal(new Bootstrap());
		Application.Run();
	}
}

class Bootstrap : GlobalEntity
{
	protected override void OnCreate()
	{
		
	}
}

// TODO: Handle issue of loading worlds.
// TODO: Test current architecture. Test everything!
