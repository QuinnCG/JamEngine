using OpenTK.Mathematics;
using System.Diagnostics;

namespace Engine.Rendering;

[DependOn<Transform>]
public class Camera : Entity
{
	/// <summary>
	/// The currently active and rendering <see cref="Camera"/>.
	/// <br>This should never be null other than before calling <see cref="Application.Launch"/>.</br>
	/// </summary>
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

	public Camera()
	{
		if (_active == null)
		{
			SetActive();
		}
		CreateComponent<Transform>();
	}

	public void SetActive()
	{
		_active = this;
	}

	internal Matrix4 GetMatrix(bool ignoreView = false)
	{
		var proj = Matrix4.CreateOrthographic(Window.AspectRatio * OrthgraphicScale, OrthgraphicScale, 0f, 1f);
		var view = Matrix4.CreateTranslation(-Transform!.Position.ToVector3());

		if (ignoreView)
		{
			return proj;
		}

		return view * proj;
	}
}
