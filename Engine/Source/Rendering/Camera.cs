using OpenTK.Mathematics;
using System.Diagnostics;

namespace Engine.Rendering;

[DependOn<Transform>]
public class Camera : Entity
{
	public static Camera Active
	{
		get
		{
			Debug.Assert(_active != null, "There must be an Active camera at any given time!");
			return _active;
		}
	}
	private static Camera? _active;

	public float OrthgraphicScale { get; set; } = 5f;

	private readonly Transform _transform;

	public Camera()
	{
		_transform = CreateComponent<Transform>();
	}

	public void SetActive()
	{
		_active = this;
	}

	internal Matrix4 GetMatrix()
	{
		var proj = Matrix4.CreateOrthographic(Window.AspectRatio * OrthgraphicScale, OrthgraphicScale, 0f, 1f);
		var view = Matrix4.CreateTranslation(-_transform.Position.ToVector3());

		return proj * view;
	}

	protected override void OnCreate()
	{
		if (_active == null)
		{
			SetActive();
		}
	}
}
