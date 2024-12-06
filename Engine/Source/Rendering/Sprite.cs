using OpenTK.Mathematics;

namespace Engine.Rendering;

public struct Sprite
{
	public Texture Texture;
	public Vector2 UVOffset;
	public Vector2 UVScale;

	public Sprite(Texture texture)
	{
		Texture = texture;
		UVOffset = Vector2.Zero;
		UVScale = Vector2.One;
	}
	public Sprite(Texture texture, Vector2 uvOffset, Vector2 uvScale)
	{
		Texture = texture;
		UVOffset = uvOffset;
		UVScale = uvScale;
	}
}
