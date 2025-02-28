namespace Engine.Rendering;

public class RenderLayer
{
	public static RenderLayer Default { get; }
	public static RenderLayer UI { get; }

	public static IEnumerable<RenderLayer> Layers => _layers;
	public static int LayerCount => _layers.Count;

	private static readonly List<RenderLayer> _layers;

	/// <summary>
	/// 0 is the furthest back with greater values being placed closer to the camera in front of the lower Order layers.
	/// </summary>
	public int Order => IndexOf(this);

	static RenderLayer()
	{
		_layers = [];

		Default = Create();
		UI = Create();
	}

	private RenderLayer() { }

	public static RenderLayer Create()
	{
		var layer = new RenderLayer();
		_layers.Add(layer);

		return layer;
	}
	public static RenderLayer CreateBehind(RenderLayer layer)
	{
		return Create().PlaceBehind(layer);
	}
	public static RenderLayer CreateInFront(RenderLayer layer)
	{
		return Create().PlaceInFront(layer);
	}

	public static int IndexOf(RenderLayer layer)
	{
		return _layers.IndexOf(layer);
	}

	public RenderLayer PlaceBehind(RenderLayer layer)
	{
		if (layer != this)
		{
			_layers.Remove(this);
			_layers.Insert(IndexOf(layer), this);
		}

		return this;
	}

	public RenderLayer PlaceInFront(RenderLayer layer)
	{
		if (layer != this)
		{
			_layers.Remove(this);
			int index = IndexOf(layer) + 1;

			if (index < _layers.Count)
			{
				_layers.Insert(index, this);
			}
			else
			{
				_layers.Add(this);
			}
		}

		return this;
	}
}
