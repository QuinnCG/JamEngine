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

	protected Transform Transform { get; }

	public Camera()
	{
		if (_active == null)
		{
			SetActive();
		}

		Transform = CreateComponent<Transform>();
	}

	public void SetActive()
	{
		_active = this;
	}

	internal Matrix4 GetMatrix(bool ignoreView = false)
	{
		var proj = Matrix4.CreateOrthographic(Window.AspectRatio * OrthgraphicScale, OrthgraphicScale, 0f, 1f);
		var view = Matrix4.CreateTranslation(-Transform.Position.ToVector3());

		if (ignoreView)
		{
			return proj;
		}

		return view * proj;
	}
}
