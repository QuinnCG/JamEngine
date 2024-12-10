using Engine;
using Engine.Rendering;
using Engine.Resources;

namespace Sandbox;

class MyWorld : World
{
	public override IEnumerable<Entity> OnLoad()
	{
		using var res = Resource.Load<TextureResource>("MyFont.bmp");
		var font = new Texture(res);

		var logo = new Logo();
		using var tilemapRes = Resource.Load<TilemapResource>("MyMap.tmx");

		return
			[
				new Tilemap(tilemapRes),
				new TimeUI(font) { Position = new(-0.4f, 0.7f) },
				logo,
				new FollowCamera(logo.Transform!) { OrthgraphicScale = 10f },
			];
	}
}
