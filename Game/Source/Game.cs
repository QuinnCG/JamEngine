using Engine;
using Engine.Rendering;
using Engine.Simulation;
using Engine.InputSystem;
using OpenTK.Mathematics;
using System.Net.NetworkInformation;

namespace Sandbox;

static class Game
{
	private static float _nextSpawnTime;

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
		cam.CreateComponent<CameraView>().OrthographicSize = 10f;

		var e1 = world.CreateEntity<MyEnt>();
		e1.Name = "Top Ent";
		world.CreateEntity<MyEnt>().SetY(1f);

		var ground = world.CreateEntity<Entity>();
		ground.Name = "Ground";
		ground.SetY(-5f);
		ground.Scale = new(30f, 5f);
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

		if (Input.IsButtonPressed(Button.Left))
		{
			Log.Info(CameraView.Current.ScreenToWorldPoint(Input.MousePosition));
		}

		if (Time.Now > _nextSpawnTime)
		{
			_nextSpawnTime = Time.Now + 0.2f;

			World.Current.CreateEntity<MyEnt>().Position = new()
			{ 
				X = Rand.Next(-3f, 3f),
				Y = 4f
			};

			Log.Info("Spawning!");
		}
	}
}

class MyEnt : Entity
{
	private SpriteRenderer _renderer;

	private float _seed;

	protected override void OnCreate()
	{
		Scale = Vector2.One * 0.3f;

		_renderer = CreateComponent<SpriteRenderer>();
		CreateComponent<Rigidbody>();
		AddComponent(new BoxCollider(Vector2.Zero, Vector2.One));

		GetComponent<BoxCollider>().Friction = 5f;

		_seed = Rand.Value;
	}

	protected override void OnUpdate()
	{
		_renderer.Color = Color4.FromHsv(new Vector4((_seed + (Time.Now * 0.1f)) % 1f, 1f, 1f, 1f));
	}
}
