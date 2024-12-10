namespace Engine.Rendering;

/// <summary>
/// Controls how sprites will be scaled for rendering; in certain contexes.
/// <br>Any time a <see cref="Texture"/>'s density must be referenced, its <see cref="Texture.Size.Y"/> stat is used.</br>
/// </summary>
public enum SpriteScalingMode
{
	/// <summary>
	/// Do not relate sprite scale to pixel density of texture.
	/// <br>1x1 meters in size then scale.</br>
	/// </summary>
	Source,
	/// <summary>
	/// X is 1 meter and Y adjusts based on image's ratio then scale.
	/// </summary>
	//FixedX,
	/// <summary>
	/// Y is 1 meter and X adjusts based on image's ratio then scale.
	/// </summary>
	//FixedY,
	/// <summary>
	/// Use the project's default Pixel Per Meter unit.
	/// </summary>
	DefaultPPM,
	/// <summary>
	/// Use <see cref="Sprite.PPM"/> for scaling.
	/// </summary>
	PPM,
}
