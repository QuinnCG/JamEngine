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

		var player = world.CreateEntity<Player>();
		player.WorldPosition = new(0f, 0.5f);
		player.GetComponent<SpriteRenderer>().Tint = Color4.Red;
		player.GetComponent<SpriteRenderer>().RenderLayer = RenderLayer.CreateBehind(RenderLayer.Default);
		player.WorldScale = Vector2.One * 0.5f;

		world.CreateEntity<Camera>();

		Window.Title = "Jam Engine - Sandbox";
		Application.RegisterGlobal<GameManager>();
		Application.Run(world);
	}
}

class GameManager : GlobalEntity
{
	protected override void OnUpdate()
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			Window.Close();
		}
	}
}

class Player : SpatialEntity
{
	public Player()
	{
		CreateComponent<SpriteRenderer>();
	}

	protected override void OnCreate()
	{
		Input.OnKeyPressed += key =>
		{
			if (key is Key.Space)
			{
				WorldScale *= 1.2f;
			}
		};
	}

	protected override void OnUpdate()
	{
		var inputDir = new Vector2()
		{
			X = Input.GetAxis(Key.A, Key.D),
			Y = Input.GetAxis(Key.S, Key.W)
		}.NormalizedOrZero();

		WorldPosition += 2f * Time.Delta * inputDir;
	}
}

// TODO: Wait system needs some work. Time seems to continue when unfocused windows (maybe glfw.gettime issue?) and cancellation doesn't seem to work (at least not for duration).
// TODO: Seprate render from update.
// TODO: Call entities async?
