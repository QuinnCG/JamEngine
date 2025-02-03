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
			.AddChild(new MyEntity() { LocalPosition = new(1f, 0f) });

		Application.Run(world);
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
