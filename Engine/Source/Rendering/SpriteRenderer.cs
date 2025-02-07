using OpenTK.Mathematics;

namespace Engine.Rendering;

public class SpriteRenderer : Component
{
	private static readonly string _shaderPath = "DefaultSprite.shader";

	private static StaticMeshBatch? _quadMesh;
	private static Shader? _shader;

	public RenderLayer RenderLayer
	{
		get => _renderLayer;
		set
		{
			if (_renderLayer != value)
			{
				_renderLayer = value;
				_hook.NotifyLayerChange();
			}
		}
	}
	public Color4 Tint { get; set; } = Color4.White;
	public Texture? Texture { get; set; }

	private RenderLayer _renderLayer = RenderLayer.Default;
	private readonly RenderHook _hook;

	private SpatialEntity? _entity;

	public SpriteRenderer()
	{
		_hook = new RenderHook(OnRender, GetRenderLayer);
	}

	internal static void Initialize()
	{
		MeshBatchBuilder.Create()
			.Quad(Vector2.Zero, Vector2.One)
			.Build(out float[] vertices, out uint[] indices);

		_quadMesh = new StaticMeshBatch().Create(vertices, indices);
		_shader = Resource.LoadEngineResource<Shader>(_shaderPath);
	}

	internal static void CleanUp()
	{
		_quadMesh!.Destroy();

		_shader!.Release();
		_shader = null;
	}

	protected override void OnCreate()
	{
		if (Entity is SpatialEntity ent)
		{
			_entity = ent;
		}
		else
		{
			Log.Error($"SpriteRenderer on entity '{Entity.Name}' must have a SpatialEntity!");
			return;
		}

		Renderer.RegisterHook(_hook);
	}

	protected override void OnDestroy()
	{
		Renderer.UnregisterHook(_hook);
	}

	private int OnRender()
	{
		if (!DoesUpdate)
		{
			return 0;
		}

		_quadMesh!.Bind();

		var mvp = Matrix4.Identity;
		mvp *= Matrix4.CreateScale(new Vector3(_entity!.WorldScale));
		mvp *= Matrix4.CreateRotationZ(-_entity!.WorldRotation * (MathF.PI / 180f));
		mvp *= Matrix4.CreateTranslation(new Vector3(_entity!.WorldPosition));

		mvp *= Camera.Active.GetMatrix();

		_shader!.Bind();
		_shader!.SetUniform("u_color", Tint);
		_shader!.SetUniform("u_mvp", mvp);

		_shader!.SetUniform("u_isTextured", Texture != null);
		Texture?.Bind();

		return _quadMesh.IndexCount;
	}

	private RenderLayer GetRenderLayer()
	{
		return _renderLayer;
	}
}
