using Engine;
using Engine.Rendering;
using System.Text;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var world = new World();
		world.CreateEntity<Player>();

		Application.Run(world);
	}
}

class Player : SpatialEntity
{
	public Player()
	{
		CreateComponent<SpriteRenderer>();
	}
}

// TODO: Wait system needs some work. Time seems to continue when unfocused windows (maybe glfw.gettime issue?) and cancellation doesn't seem to work (at least not for duration).
// TODO: Seprate render from update.
// TODO: Call entities async?
