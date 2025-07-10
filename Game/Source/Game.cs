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
	private static float _timeScaleBeforePause;

	static void Main()
	{
		Application.OnLoad += OnLoad;
		Application.OnUpdate += OnUpdate;

		Application.Run();
	}

	private static void OnLoad()
	{
		var world = World.Create(new Vector2(0f, -9.8f));

		world.CreateEntity<DebugCamera>();

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
		}

		// Toggle time.
		if (Input.IsKeyPressed(Key.Space))
		{
			_timeScaleBeforePause = Time.Scale;
			Time.Scale = (Time.Scale > 0f) ? 0f : _timeScaleBeforePause;
		}

		if (Input.IsKeyPressed(Key.D1))
		{
			Time.Scale = 1f;
		}
		else if (Input.IsKeyPressed(Key.D2))
		{
			Time.Scale = 1.5f;
		}
		else if (Input.IsKeyPressed(Key.D3))
		{
			Time.Scale = 2f;
		}
		else if (Input.IsKeyPressed(Key.D4))
		{
			Time.Scale = 3f;
		}
		else if (Input.IsKeyPressed(Key.D5))
		{
			Time.Scale = 5f;
		}
		else if (Input.IsKeyPressed(Key.D6))
		{
			Time.Scale = 10f;
		}
	}
}
