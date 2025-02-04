using Engine;
using Engine.Rendering;
using OpenTK.Mathematics;
using System.Text;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var world = new World();

		world.CreateEntity<Player>().GetComponent<SpriteRenderer>().Tint = new(1f, 1f, 1f, 0.5f);

		var player2 = world.CreateEntity<Player>();
		player2.WorldPosition = new(0f, 0.5f);
		player2.GetComponent<SpriteRenderer>().Tint = Color4.Red;
		player2.GetComponent<SpriteRenderer>().RenderLayer = RenderLayer.CreateBehind(RenderLayer.Default);
		player2.WorldScale = Vector2.One * 0.5f;

		var cam = world.CreateEntity<Camera>();
		cam.WorldRotation = -45f;

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
