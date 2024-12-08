using Engine;
using Engine.Rendering;
using Engine.Resources;
using Engine.UI;
using OpenTK.Mathematics;
using System.Drawing;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		new MyWorld().Load();
		Application.OnLaunch += OnLaunch;
		Application.OnUpdate += OnUpdate;

		Window.Title += " - Sandbox";
		Application.Launch();
	}

	private static void OnLaunch()
	{
		using var text = Resource.Load<TextResource>("Sub Folder/MySubRes1.txt");
		Log.Info(text);
	}

	private static void OnUpdate()
	{
		if (Input.IsPressed(Key.Escape))
		{
			Application.Quit();
		}
	}
}

class MyWorld : World
{
	public override IEnumerable<Entity> OnLoad()
	{
		var a = new UIEntity()
		{
			new UILayout()
			{
				new UIEntity(Color4.Red),
				new UIEntity(Color4.Blue)
			}
		};

		return
			[
				new MyCamera(),
				new MySprite(),
				a
			];
	}
}

class MySprite : Entity
{
	private readonly float _moveSpeed = 3f;
	private Texture? _tex;

	protected override void OnCreate()
	{
		CreateComponent<Transform>();
		CreateComponent<SpriteRenderer>().Tint = Color.Red;

		using var res = Resource.Load<TextureResource>("Logo.png");
		var texture = new Texture(res);

		GetComponent<SpriteRenderer>().Sprite = new Sprite(texture);
		_tex = texture;
	}

	protected override void OnUpdate()
	{
		float hue = MathX.Range(-1f, 1f, 0f, 1f, MathX.Sin(Time.Now));
		GetComponent<SpriteRenderer>().Tint = Color4.FromHsv(new(hue, 1f, 1f, 1f));

		var moveDir = new Vector2()
		{
			X = Input.GetAxis(Key.A, Key.D),
			Y = Input.GetAxis(Key.S, Key.W)
		}.NormalizedOrZero();

		GetComponent<Transform>().Position += moveDir * _moveSpeed * Time.Delta;
	}

	protected override void OnDestroy()
	{
		_tex?.Dispose();
	}
}

class MyCamera : Camera
{
	protected override void OnUpdate()
	{
		Transform.SetPositionY(MathX.Cos(Time.Now * 0.5f));
	}
}
