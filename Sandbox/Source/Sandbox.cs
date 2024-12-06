using Engine;
using Engine.Rendering;
using Engine.Resources;
using OpenTK.Mathematics;
using System.Drawing;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		new MyWorld().Load();
		Application.OnLaunch += OnLaunch;

		Window.Title += " - Sandbox";
		Application.Launch();
	}

	private static void OnLaunch()
	{
		using var text = Resource.Load<TextResource>("Sub Folder/MySubRes1.txt");
		Log.Info(text);
	}
}

class MyWorld : World
{
	public override IEnumerable<Entity> OnLoad()
	{
		return 
			[
				new MyCamera(),
				new MySprite()
			];
	}
}

class MySprite : Entity
{
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

		var transform = GetComponent<Transform>();

		transform.SetPositionX(MathX.Sin(Time.Now));
		transform.Rotation = Time.Now * 180f;
		transform.ScaleUniform = ((MathX.Cos(Time.Now) + 1f) / 2f) + 0.5f;
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
