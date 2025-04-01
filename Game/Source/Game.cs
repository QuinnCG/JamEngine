using Engine;

namespace Sandbox;

static class Game
{
	static void Main()
	{
		var world = new World();
		World.Load(world);

		world.CreateEntity<SpatialEntity>();

		Application.Run();
	}
}
