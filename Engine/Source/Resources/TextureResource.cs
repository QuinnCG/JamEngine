using OpenTK.Mathematics;
using StbiSharp;

namespace Engine.Resources;

public record TextureResource : IResource
{
	public byte[] Pixels { get; private set; } = [];
	public int Channels { get; private set; } = 0;
	public Vector2i Size { get; private set; }

	public void Load(byte[] data)
	{
		Stbi.SetFlipVerticallyOnLoad(true);

		var span = new ReadOnlySpan<byte>(data);
		var image = Stbi.LoadFromMemory(span, 4);

		Pixels = image.Data.ToArray();
		Channels = image.NumChannels;
		Size = new(image.Width, image.Height);
	}

	public bool IsLoaded()
	{
		return Pixels.Length > 0;
	}

	public void Dispose()
	{
		Pixels = [];
		Channels = 0;
		Size = Vector2i.Zero;

		GC.SuppressFinalize(this);
	}
}
