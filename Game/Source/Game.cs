using Engine;
using Engine.Rendering;
using OpenTK.Mathematics;

namespace Sandbox;

static class Game
{
	static void Main()
	{
		Application.OnLoad += OnLoad;
		Application.OnUpdate += OnUpdate;

		Application.Run();
	}

	private static void OnLoad()
	{
		var world = World.Create();
		world.Load();

		var cam = world.CreateEntity<Entity>();
		cam.CreateComponent<CameraView>();

		world.CreateEntity<MyEnt>();
	}

	private static void OnUpdate()
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			Application.Quit();
		}
	}
}

class MyEnt : Entity
{
	private SpriteRenderer _renderer;

	protected override void OnCreate()
	{
		_renderer = CreateComponent<SpriteRenderer>();
	}

	protected override void OnUpdate()
	{
		_renderer.Color = Color4.FromHsv(new Vector4(Time.Now % 1f, 1f, 1f, 1f));
		Rotation = Time.Now * 30f;
	}
}
