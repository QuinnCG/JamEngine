using OpenTK.Mathematics;

namespace Engine.Rendering;

public class SpriteRenderer : Component, IRenderable
{
	public const string DefaultShaderPath = "DefaultSprite.shader";

	public Texture Texture { get; set; } = null;
	public Color4 Color { get; set; } = Color4.White;
	public RenderLayer Layer { get; set; } = RenderLayer.Default;
	/// <summary>
	/// The shader to use for rendering the sprite.<br/>
	/// Upon <see cref="OnCreate"/>, if this is null, it will be set to the shader at the path "<c><see cref="DefaultShaderPath"/></c>".
	/// </summary>
	public Shader Shader { get; set; }

	public override string ToString()
	{
		return base.ToString() + $" <Color: ({Color.R}, {Color.G}, {Color.B}, {Color.A}), Layer: {Layer}, Texture: N/A>";
	}

	public RenderLayer GetRenderLayer() => Layer;

	protected override void OnCreate()
	{
		Shader ??= Resource.LoadEngineResource<Shader>(DefaultShaderPath);
		Renderer.Register(this);
	}

	public int Render()
	{
		if (Shader == null)
		{
			throw new NullReferenceException("Shader is not set for SpriteRenderer!");
		}

		var mvp = Matrix4.CreateScale(Entity.Scale.X, Entity.Scale.Y, 1f)
			* Matrix4.CreateRotationZ(-Entity.Rotation * MathX.DegToRad)
			* Matrix4.CreateTranslation(Entity.Position.X, Entity.Position.Y, 0f)
			* CameraView.Current.ViewProjectionMatrix;

		Shader.SetUniform("u_mvp", mvp);
		Shader.SetUniform("u_color", Color);

		Shader.SetUniform("u_useTex", Texture != null);
		Texture?.Bind();

		return SpriteManager.Instance.BindSpriteMesh();
	}

	protected override void OnDestroy()
	{
		if (Shader != null && Shader.RelativePath == DefaultShaderPath)
		{
			Shader.Release();
			Shader = null;
		}

		Renderer.Unregister(this);
	}
}
