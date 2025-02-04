using OpenTK.Mathematics;

namespace Engine.Rendering;

public class Camera : SpatialEntity
{
	public static Camera Active
	{
		get
		{
			if (_active != null)
			{
				return _active;
			}

			Log.Error(Renderer.LogCategory, "There is no active camera!");
			return null;
		}
		set => _active = value;
	}
	private static Camera? _active;

	public float OrthgraphicSize { get; set; } = 5f;

	public void SetActive()
	{
		_active = this;
	}

	internal Matrix4 GetMatrix()
	{
		return
			Matrix4.CreateScale(new Vector3(WorldScale)) *
			Matrix4.CreateRotationZ(WorldRotation * (MathF.PI / 180f)) *
			Matrix4.CreateTranslation(-new Vector3(WorldPosition)) *
			Matrix4.CreateOrthographic(Window.Ratio * OrthgraphicSize, OrthgraphicSize, 0f, 1f);
	}

	protected override void OnCreate()
	{
		SetActive();
	}

	protected override void OnDestroy()
	{
		if (_active == this)
		{
			_active = null;
		}
	}
}
