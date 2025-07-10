using Engine;
using Engine.Rendering;
using Engine.Simulation;
using OpenTK.Mathematics;

namespace Sandbox;

class MyEnt : Entity
{
	private readonly SpriteRenderer _renderer;
	private readonly float _seed;

	public MyEnt()
	{
		Scale = Vector2.One * 0.3f;

		_renderer = CreateComponent<SpriteRenderer>();
		_renderer.Alpha = 0f;

		_seed = Rand.Value;
		_renderer.Texture = Resource.Load<Texture>("Logo.png");
	}

	protected override void OnCreate()
	{
		CreateComponent<Rigidbody>();
		AddComponent(new BoxCollider(Vector2.Zero, Vector2.One));
		GetComponent<BoxCollider>().Friction = 5f;
	}

	protected override void OnUpdate()
	{
		// Fade in.
		if (_renderer.Alpha < 1f)
		{
			_renderer.Alpha += Time.Delta * 2f;
		}

		_renderer.Color = Color4.FromHsv(new Vector4((_seed + (Time.Now * 0.1f)) % 1f, 1f, 1f, _renderer.Alpha));
	}

	protected override void OnDestroy()
	{
		_renderer.Texture.Release();
	}
}
