using Engine;
using Engine.Rendering;
using System.Drawing;

namespace Sandobx;

static class Program
{
	private static void Main()
	{
		new MyWorld().Load();

		Window.Title += " - Sandbox";
		Application.Launch();
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
