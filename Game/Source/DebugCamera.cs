using Engine;
using Engine.Rendering;
using Engine.InputSystem;
using OpenTK.Mathematics;

namespace Sandbox;

class DebugCamera : Entity
{
	public float OrhographicSize { get => _view.OrthographicSize; set => _view.OrthographicSize = value; }
	public float PanSpeed { get; set; } = 2f;
	public float ZoomSpeed { get; set; } = 2f;

	private CameraView _view;


	protected override void OnCreate()
	{
		_view = CreateComponent<CameraView>();
	}

	protected override void OnUpdate()
	{
		var inputDir = new Vector2()
		{
			X = Input.GetAxis(Key.A, Key.D),
			Y = Input.GetAxis(Key.S, Key.W)
		};

		if (inputDir.LengthSquared != 0f)
		{
			inputDir.Normalize();
		}

		Position += Time.UnscaledDelta * inputDir * PanSpeed * OrhographicSize / 3f;

		OrhographicSize += -Input.ScrollDelta * ZoomSpeed * OrhographicSize / 10f;
		OrhographicSize = MathX.Clamp(OrhographicSize, 0.1f, 50f);
	}
}
