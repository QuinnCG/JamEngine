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

		world.CreateEntity<MyEnt>().SetX(-1f);
		world.CreateEntity<MyEnt>().SetX(1f);

		var ground = world.CreateEntity<Entity>();
		ground.SetY(-2.5f);
		ground.Scale = new(10f, 0.5f);
		ground.CreateComponent<SpriteRenderer>();
		ground.AddComponent(new Rigidbody(RigidbodyType.Dynamic));
		ground.CreateComponent<BoxCollider>();

		world.Load();
	}

	private static void OnUpdate()
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			Application.Quit();
		}
	}
}

// FIX: Rigidbodies launch way from each other, as if their actual colliders are spawning within one another. Rigidbodies don't collide with static ground.

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
		GetComponent<Rigidbody>().AngularVelocity = Position.X > 0f ? 10f : -10f;
	}
}
