using OpenTK.Mathematics;

namespace Engine.Rendering;

public struct Sprite
{
	public Texture Texture;
	public Vector2 UVOffset;
	public Vector2 UVScale;

	// TODO: Implement alternative scaling mode options.
	// Also, make default scaling mode be DefaultPPM not PPM.
	public SpriteScalingMode ScalingMode = SpriteScalingMode.DefaultPPM;
	/// <summary>
	/// Pixels-per-meter.
	/// <br>Where applicable, this value will be used to adjust the base size of a rendered sprite.</br>
	/// <br>This value will only be used if <see cref="ScalingMode"/> is set to <see cref="SpriteScalingMode.PPM"/>.</br>
	/// </summary>
	public int PPM = 16;

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

	/// <summary>
	/// Using <see cref="ScalingMode"/>, calculate the scaling factor for this <see cref="Sprite"/>.
	/// </summary>
	/// <returns>A factor that can be multiplied by a quad's scale.
	/// <br>The factor can be any non-zero value; barring an error.</br></returns>
	public readonly float GetScalingFactor()
	{
		switch (ScalingMode)
		{
			case SpriteScalingMode.Source:
			{
				return 1f;
			}
			case SpriteScalingMode.PPM:
			{
				return (float)Texture.Size.Y / PPM;
			}
			case SpriteScalingMode.DefaultPPM:
			{
				return (float)Texture.Size.Y / Renderer.DefaultPPM;
			}
		}

		return 1f;
	}
}
