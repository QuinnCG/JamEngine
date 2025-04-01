namespace Engine.Rendering;

public class RenderLayer : IComparable<RenderLayer>
{
	public static readonly RenderLayer Default = new();
	private static readonly List<RenderLayer> Layers = [ Default ];

	public int CompareTo(RenderLayer? other)
	{
		if (other == this)
		{
			return 0;
		}

		if (other == null)
		{
			return 1;
		}

		int a = Layers.IndexOf(this);
		int b = Layers.IndexOf(other);

		if (a > b)
		{
			return 1;
		}
		else
		{
			return -1;
		}
	}
}
