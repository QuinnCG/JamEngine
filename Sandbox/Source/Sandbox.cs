using Engine;
using Engine.Rendering;
using Engine.UI;
using OpenTK.Mathematics;
using System.Text;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var world = new World();

		var player = world.CreateEntity<Player>();
		player.GetComponent<SpriteRenderer>().Tint = Color4.Red;
		player.WorldScale = Vector2.One * 0.5f;

		world.CreateEntity<Camera>();

		var canvas = world.CreateEntity<UICanvas>();
		var text = new TextBlock("Hello World") { Color = Color4.Blue };
		canvas.AddChild(text);

		var bg = new Image(new(1f, 1f, 1f, 0.5f));
		canvas.AddChild(bg);

		Renderer.ClearColor = Color4.Gray;
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
				var res = Resource.Load<Texture>("Logo.png");
				res.IsFiltered = !res.IsFiltered;
			}
		};

		GetComponent<SpriteRenderer>().Texture = Resource.Load<Texture>("Logo.png");
	}

	protected override void OnUpdate()
	{
		var inputDir = new Vector2()
		{
			X = Input.GetAxis(Key.A, Key.D),
			Y = Input.GetAxis(Key.S, Key.W)
		}.NormalizedOrZero();

		WorldPosition += 2f * Time.Delta * inputDir;

		Camera.Active.OrthgraphicSize += -Input.ScrollDelta * 0.5f;
		Camera.Active.OrthgraphicSize = MathX.Clamp(Camera.Active.OrthgraphicSize, 0.2f, 30f);

		Camera.Active.LocalPositionY = MathX.Sin(Time.Now);
	}
}

// TODO: Wait system needs some work. Time seems to continue when unfocused windows (maybe glfw.gettime issue?) and cancellation doesn't seem to work (at least not for duration).
// TODO: Seprate render from update.
// TODO: Call entities async?
// TODO: In the sprite renderrer, there's texture repeating but no way to scale UVs. Also, need UV scaling for use with spritesheets.
