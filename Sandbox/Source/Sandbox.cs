using Engine;
using System.Text;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var world = new World();
		world.CreateEntity<MyEntity>().AddTag<DummyTag>()
			.AddChild(new SpatialEntity() { LocalPosition = new(1f, 1f)})
			.AddChild(new MyEntity() { LocalPosition = new(1f, 0f) }
				.AddChild(new MyEntity().AddTag<DummyTag2>()));

		world.CreateEntity<Player>();

		Application.RegisterGlobal<TagSearcher>();
		Application.Run(world);
	}
}

class TagSearcher : GlobalEntity
{
	protected override void OnCreate()
	{
		//var player = World.LoadedWorlds.First().GetEntitiesWithTag<PlayerTag>().First();
		//Log.Info(player);

		var ent = World.LoadedWorlds.First().GetEntitiesOfType<MyEntity>().First();
		ent.RemoveChild(ent.GetChild(0));
	}
}

class PlayerTag : Tag { }

class DummyTag : Tag { }

class DummyTag2 : Tag { }

class Player : SpatialEntity
{
	public Player()
	{
		AddTag<PlayerTag>();
	}

	protected override async void OnCreate()
	{
		Log.Info("In 5s will execute player's code...");
		await Wait.Duration(5f);
		Log.Info("Player delayed execution wasn't cancelled in time!");
	}

	protected override void OnDestroy()
	{
		Log.Info("Player destroyed!");
	}
}

class MyEntity : SpatialEntity
{
	protected override async void OnCreate()
	{
		//var builder = ListChildren(this, new StringBuilder("\n"));
		//Log.Info(builder.ToString());

		await Wait.Duration(2f);
		World.GetEntitiesOfType<Player>().FirstOrDefault()?.Destroy();
		Log.Info("Cancelling player!");
	}

	// TODO: Make this an entity method for reuse? But without positional data. Maybe serialize [Expose] fields and properties.
	private static StringBuilder ListChildren(SpatialEntity parent, StringBuilder builder, int depth = 0)
	{
		builder.AppendLine(new string(' ', depth * 3) + parent.Name + $" ({parent.WorldPosition})");

		foreach (var child in parent.Children)
		{
			ListChildren((SpatialEntity)child, builder, depth + 1);
		}

		return builder;
	}
}

// TODO: Even child entities should exist in world's entity cache. World should have unqiue cache for entities it must directly update.
// TODO: Resource system.
// TODO: Wait system needs some work. Time seems to continue when unfocused windows (maybe glfw.gettime issue?) and cancellation doesn't seem to work (at least not for duration).
