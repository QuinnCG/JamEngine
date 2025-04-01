using Engine;
using Engine.Rendering;

namespace Sandbox;

static class Game
{
	static void Main()
	{
		Application.OnLoad += OnLoad;
		Application.Run();
	}

	private static void OnLoad()
	{
		var world = new World();
		World.Load(world);

		var ent = world.CreateEntity<SpatialEntity>();
		ent.CreateComponent<SpriteRenderer>();
	}
}
