using DotTiled;
using DotTiled.Serialization;
using Engine.Rendering;
using Engine.Resources;
using OpenTK.Mathematics;

namespace Engine;

// TODO: Support tilemap chunk culling.
// TODO: Support invisible tilemaps for collision walls.
// TODO: Support invisible tilemaps for pathfinding.

public class Tilemap : Entity
{
	private readonly Transform _transform;

	private readonly TilemapResource? _resource;
	private readonly Map _map;

	private Mesh? _mesh;
	private readonly Texture _texture;
	private readonly RenderObject _renderObj;

	public Tilemap(TilemapResource tilemap)
	{
		_transform = CreateComponent<Transform>();
		_map = LoadMap(tilemap);

		_texture = new Texture(Resource.Load<TextureResource>("MyFont.bmp"));

		RegenerateMesh();

		_renderObj = new RenderObject()
		{
			GetIndexCount = () => _mesh!.IndexCount,
			OnBind = OnBind,

			GetPosition = () => _transform!.Position,
			GetRotation = () => _transform!.Rotation,
			GetScale = () => _transform!.Scale
		};
	}

	protected override void OnCreate()
	{
		Renderer.Register(_renderObj);
	}

	protected override void OnDestroy()
	{
		Renderer.Unregister(_renderObj);
	}

	private Map LoadMap(TilemapResource res)
	{
		Tileset ResolveTileset(string source)
		{
			using var tilesetReader = new TilesetReader(_resource!.Data, ResolveTileset, ResolveTemplate, ResolveCustomType);
			return tilesetReader.ReadTileset();
		}
		Template ResolveTemplate(string source)
		{
			// TODO: Implement template support for tilemaps.
			using TemplateReader templateReader = new TemplateReader(string.Empty, ResolveTileset, ResolveTemplate, ResolveCustomType);
			return templateReader.ReadTemplate();
		}
		Optional<ICustomTypeDefinition> ResolveCustomType(string name)
		{
			// TODO: Support tilemap type defs.
			return Optional<ICustomTypeDefinition>.Empty;
		}

		using var mapReader = new MapReader(res.Data, ResolveTileset, ResolveTemplate, ResolveCustomType);
		return mapReader.ReadMap();
	}

	private void OnBind()
	{
		_texture.Bind();
		_mesh!.Bind();
	}

	private void RegenerateMesh()
	{
		_mesh?.Dispose();
		var builder = new MeshBuilder();

		var opt = (_map.Layers[0] as TileLayer)!.Data;
		var data = opt.Value;

		Tileset tileset = _map.Tilesets[0];

		foreach (Chunk chunk in data.Chunks.Value)
		{
			foreach (uint gid in chunk.GlobalTileIDs)
			{
				if (gid == 0) continue;
				int id = (int)(gid - tileset.FirstGID);

				var tileLocalPos = new Vector2i()
				{
					X = id % (int)chunk.Width,
					Y = id / (int)chunk.Height
				};

				// FIX: Tilemap needs a WORKING way to convert gid of a tile to a world pos.

				var chunkPos = new Vector2i(chunk.X, chunk.Y);
				var tileGlobalPos = chunkPos + tileLocalPos;

				// invert Y-coord so that Y is up.
				//tileGlobalPos.Y = (int)_map.Height - tileGlobalPos.Y;

				// Build quad.
				builder.Quad(tileGlobalPos, 0f, new(0.9f));
			}
		}

		_mesh = builder.Build(isStatic: true);
	}
}
