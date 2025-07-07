using Engine;
using Engine.Rendering;
using Engine.Simulation;
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
		var world = World.Create(new Vector2(0f, -9.8f));

		var cam = world.CreateEntity<Entity>();
		cam.CreateComponent<CameraView>();

		var e1 = world.CreateEntity<MyEnt>();
		e1.SetX(0-1);
		world.CreateEntity<MyEnt>().SetX(1f);

		var ground = world.CreateEntity<Entity>();
		ground.SetY(-2.5f);
		ground.Scale = new(10f, 0.5f);
		ground.CreateComponent<SpriteRenderer>();
		ground.AddComponent(new Rigidbody(RigidbodyType.Dynamic));
		ground.CreateComponent<BoxCollider>();

		world.Load();

		e1.GetComponent<Rigidbody>().OnCollide += c =>
		{
			Log.Info($"Hit! {c.Entity.GetType().Name}");
			return true;
		};
	}

	private static void OnUpdate()
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			Application.Quit();
		}
	}
}

// FIX: Not colliding with ground.

class MyEnt : Entity
{
	private SpriteRenderer _renderer;

	protected override void OnCreate()
	{
		_renderer = CreateComponent<SpriteRenderer>();
		CreateComponent<Rigidbody>();
		AddComponent(new BoxCollider(Vector2.Zero, Vector2.One));
	}

	protected override void OnUpdate()
	{
		_renderer.Color = Color4.FromHsv(new Vector4(Time.Now % 1f, 1f, 1f, 1f));
	}
}
