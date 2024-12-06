using Engine;
using Engine.Rendering;
using Engine.Resources;
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
				new Camera(),
				new MySprite()
			];
	}
}

class MySprite : Entity
{
	protected override async void OnCreate()
	{
		CreateComponent<Transform>();
		CreateComponent<SpriteRenderer>().Tint = Color.Red;
		await Wait.Seconds(2.5f);
		GetComponent<SpriteRenderer>().Tint = Color.Yellow;
	}

	protected override void OnFixedUpdate()
	{
		var transform = GetComponent<Transform>();
		transform.SetPositionX(MathX.Sin(Time.Now));
	}
}
