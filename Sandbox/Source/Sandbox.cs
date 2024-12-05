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
		using var binary = Resource.Load<BinaryResource>("MyRes.txt");
	}
}

class MyWorld : World
{
	public override IEnumerable<Entity> OnLoad()
	{
		return [new MySprite()];
	}
}

class MySprite : Entity
{
	protected override void OnCreate()
	{
		CreateComponent<SpriteRenderer>().Tint = Color.Red;
	}
}
