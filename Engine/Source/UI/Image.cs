using Engine.Rendering;
using OpenTK.Mathematics;

namespace Engine.UI;

public class Image : UIEntity
{
	private static StaticMeshBatch? _mesh;
	private static Shader? _shader;

	public Texture? Texture { get; set; }
	public Color4 Color { get; set; } = Color4.White;

	internal static void Initialize()
	{
		MeshBatchBuilder.Create()
			.Quad(new(0f), new(1f))
			.Build(out float[] vertices, out uint[] indices);

		_mesh = new StaticMeshBatch().Create(vertices, indices);
		_shader = Resource.Load<Shader>("DefaultSprite.shader");
	}

	internal static void CleanUp()
	{
		_mesh!.Destroy();
		_shader!.Release();
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		Renderer.RegisterHook(RenderHook);
	}

	protected override int OnRender()
	{
		_mesh!.Bind();
		_shader!.Bind();

		_shader!.SetUniform("u_color", Color);
		_shader!.SetUniform("u_mvp", Canvas.CalculateMatrix(this));

		return _mesh!.IndexCount;
	}
}
