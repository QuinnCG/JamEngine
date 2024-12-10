using Engine;
using Engine.Rendering;
using Engine.Resources;
using OpenTK.Mathematics;
using System.Drawing;

namespace Sandbox;

class Logo : Entity
{
	private readonly float _moveSpeedFactor = 0.5f;
	private Camera? _camera;

	public Logo()
	{
		CreateComponent<Transform>();
	}

	protected override void OnCreate()
	{
		_camera = Camera.Active;
		CreateComponent<SpriteRenderer>().Tint = Color.Red;

		using var res = Resource.Load<TextureResource>("Logo.png");
		var texture = new Texture(res);

		GetComponent<SpriteRenderer>().Sprite = new Sprite(texture)
		{
			ScalingMode = SpriteScalingMode.Source
		};
	}

	protected override void OnUpdate()
	{
		float hue = MathX.Range(-1f, 1f, 0f, 1f, MathX.Sin(Time.Now));
		GetComponent<SpriteRenderer>().Tint = Color4.FromHsv(new(hue, 1f, 1f, 1f));

		var moveDir = new Vector2()
		{
			X = Input.GetAxis(Key.A, Key.D),
			Y = Input.GetAxis(Key.S, Key.W)
		}.NormalizedOrZero();

		GetComponent<Transform>().Position += moveDir * _moveSpeedFactor * Time.Delta * _camera!.OrthgraphicScale;
	}
}
