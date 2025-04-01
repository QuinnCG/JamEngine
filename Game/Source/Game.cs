using Engine;
using Engine.Rendering;

namespace Sandbox;

static class Game
{
	static void Main()
	{
		var world = new World();
		World.Load(world);

		var ent = world.CreateEntity<SpatialEntity>();
		ent.CreateComponent<SpriteRenderer>();

		Application.Run();
	}
}
