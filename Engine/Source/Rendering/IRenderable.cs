using OpenTK.Mathematics;

namespace Engine.Rendering;

/// <summary>
/// Once inherited from this, an object can be registered with the <c>Renderer</c> for rendering.
/// </summary>
public interface IRenderable
{
	internal Vector2 Position_Internal => Position;
	internal float Rotation_Internal => Rotation;
	internal Vector2 Scale_Internal => Scale;

	internal Texture? Texture_Internal => Texture;
	internal Vector2 UVOffset_Internal => UVOffset;
	internal Vector2 UVScale_Internal => UVScale;

	internal Color4 Tint_Internal => Tint;
	internal int IndexCount_Internal => IndexCount;

	/// <summary>
	/// The position in world-space.
	/// </summary>
	protected Vector2 Position { get; }
	protected float Rotation { get; }
	protected Vector2 Scale { get; }

	protected Texture? Texture { get; }
	protected Vector2 UVOffset { get; }
	protected Vector2 UVScale { get; }

	/// <summary>
	/// This value is multiplied in the shader with the texture.
	/// <br>If no textured was provided, this value becomes the effective color.</br>
	/// </summary>
	protected Color4 Tint { get; }
	/// <summary>
	/// The number of indices this <c>IRenderable</c> will have while rendering.
	/// </summary>
	protected int IndexCount { get; }

	internal void Bind_Internal() => Bind();
	internal void CleanUp_Internal() => CleanUp();

	/// <summary>
	/// Called when the <c>Renderer</c> is about to render this.
	/// </summary>
	protected void Bind();
	/// <summary>
	/// If this is registered with the <c>Renderer</c> then this will be called when the <c>Renderer</c> executes its cleanup at the end of the <c>Application</c>'s life.
	/// </summary>
	protected void CleanUp();
}
