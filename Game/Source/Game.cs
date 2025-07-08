using Engine;
using Engine.Rendering;
using Engine.Simulation;
using Engine.InputSystem;
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
		cam.Name = "Camera";
		cam.CreateComponent<CameraView>();

		var e1 = world.CreateEntity<MyEnt>();
		e1.Name = "Top Ent";
		world.CreateEntity<MyEnt>().SetY(1f);

		var ground = world.CreateEntity<Entity>();
		ground.SetY(-2.5f);
		ground.Scale = new(10f, 0.5f);
		ground.CreateComponent<SpriteRenderer>().Color = new(0.2f, 0.2f, 0.2f, 1f);
		ground.AddComponent(new Rigidbody(RigidbodyType.Static));
		ground.AddComponent(new BoxCollider(Vector2.Zero, new(10f, 1f)));

		world.Load();

		e1.GetComponent<Rigidbody>().OnCollide += c =>
		{
			Log.Info($"Hit! {c.Entity}");
			return true;
		};

		world.PrintHierarchy();
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

	private float _seed;
	private bool _fixed;

	protected override void OnCreate()
	{
		_renderer = CreateComponent<SpriteRenderer>();
		CreateComponent<Rigidbody>();
		AddComponent(new BoxCollider(Vector2.Zero, Vector2.One));

		if (World.EntityCount > 2)
		{
			_fixed = true;
		}

		_seed = Rand.Value;
	}

	protected override void OnUpdate()
	{
		_renderer.Color = Color4.FromHsv(new Vector4((_seed + (Time.Now * 0.1f)) % 1f, 1f, 1f, 1f));

		if (_fixed)
		{
			SetY(0f);
		}
	}
}
