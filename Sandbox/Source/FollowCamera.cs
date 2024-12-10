using Engine;
using Engine.Rendering;

namespace Sandbox;

class FollowCamera(Transform target) : Camera
{
	protected override void OnUpdate()
	{
		Transform!.Position = target.Position;

		OrthgraphicScale -= Input.ScrollDelta * OrthgraphicScale * 0.1f;
		OrthgraphicScale = MathX.Clamp(OrthgraphicScale, 1.5f, 50f);
	}
}
