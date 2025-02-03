using Engine;
using System.Text;

namespace Sandbox;

static class Sandbox
{
	static void Main()
	{
		var world = new World();
		world.CreateEntity<MyEntity>()
			.AddChild(new SpatialEntity() { LocalPosition = new(1f, 1f)})
			.AddChild(new MyEntity() { LocalPosition = new(1f, 0f) }
				.AddChild(new MyEntity()));

		world.CreateEntity<Player>();

		Application.RegisterGlobal<TagSearcher>();
		Application.Run(world);
	}
}

class TagSearcher : GlobalEntity
{
	protected override void OnCreate()
	{
		var player = World.LoadedWorlds.First().GetEntitiesWithTag<PlayerTag>().First();
		Log.Info(player);

		var ent = World.LoadedWorlds.First().GetEntitiesOfType<MyEntity>().First();
		ent.RemoveChild(ent.GetChild(0));
	}
}

class PlayerTag : Tag { }

class Player : SpatialEntity
{
	public Player()
	{
		AddTag<PlayerTag>();
	}
}

class MyEntity : SpatialEntity
{
	protected override void OnCreate()
	{
		var builder = ListChildren(this, new StringBuilder("\n"));
		Log.Info(builder.ToString());
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

// TODO: Handle issue of loading worlds.
// TODO: Test current architecture. Test everything!
