using Engine.Rendering;
using OpenTK.Mathematics;

namespace Engine.UI;

public class UICanvas : SpatialEntity
{
	public CanvasMode Mode { get; set; } = CanvasMode.ScreenSpace;
	public RenderLayer Layer { get; set; } = RenderLayer.UI;

	private Matrix4 _camMat;
	private RenderHook? _renderHook;

	protected override void OnCreate()
	{
		_renderHook = new RenderHook(OnRender, () => Layer);
		Renderer.RegisterHook(_renderHook!);

		foreach (var child in Children)
		{
			if (child is UIEntity ui)
			{
				ui.SetCanvas(this);
			}
		}
	}

	private int OnRender()
	{
		var mat = Camera.Active.GetMatrix();

		if (_camMat != mat)
		{
			RegenerateLayout();
			_camMat = mat;
		}

		return 0;
	}

	protected override void OnDestroy()
	{
		Renderer.UnregisterHook(_renderHook!);
	}

	/// <summary>
	/// Calculates the model-view-projection matrix, used for rendering, for a given <see cref="UIEntity"/> that's a child of this <see cref="UICanvas"/>.<br/>
	/// Accounts for the <see cref="CanvasMode"/> too.
	/// </summary>
	public Matrix4 CalculateMatrix(UIEntity entity)
	{
		var model = CalculateModelMatrix(entity);
		
		if (Mode is CanvasMode.ScreenSpace)
		{
			var proj = Camera.Active.GetProjectionMatrix();
			return model * proj;
		}
		else
		{
			var canvasModelMat = Matrix4.Identity;
			model *= Matrix4.CreateScale(new Vector3(WorldScale));
			model *= Matrix4.CreateRotationZ(MathF.PI / 180f * -WorldRotation);
			model *= Matrix4.CreateTranslation(new Vector3(WorldPosition));

			return model * canvasModelMat * Camera.Active.GetMatrix();
		}
	}

	private static Matrix4 CalculateModelMatrix(UIEntity entity)
	{
		var rect = entity.Rect;
		var model = Matrix4.Identity;

		model *= Matrix4.CreateScale(new Vector3(rect.Size * entity.RenderScale));
		model *= Matrix4.CreateRotationZ(MathF.PI / 180f * -entity.RenderRotation);
		model *= Matrix4.CreateTranslation(new Vector3(rect.Center + entity.RenderPosition));

		return model;
	}

	/// <summary>
	/// Calls <see cref="OnRegenerateLayout"/> which will trigger each <see cref="UIEntity"/> to recalculate its layout.
	/// </summary>
	public void RegenerateLayout()
	{
		foreach (var child in Children)
		{
			if (child is UIEntity ui)
			{
				ui.RegenerateLayout();
				ui.SetRect_Internal(CalculateChildRect());
			}
		}
	}

	protected override void OnChildAdded(Entity child)
	{
		if (IsCreated && child is UIEntity ui)
		{
			ui.SetCanvas(this);
			RegenerateLayout();
		}
	}

	protected override void OnChildRemoved(Entity child)
	{
		if (IsCreated && child is UIEntity)
		{
			RegenerateLayout();
		}
	}

	private UIRect CalculateChildRect()
	{
		if (Mode is CanvasMode.ScreenSpace)
		{
			float orthoScale = Camera.Active.OrthgraphicSize;

			return new UIRect()
			{
				Center = Vector2.Zero,
				Size = new(Window.Ratio * orthoScale, orthoScale)
			};
		}
		else
		{
			// TODO: Implement world space.
			throw new NotImplementedException();
		}
	}
}
