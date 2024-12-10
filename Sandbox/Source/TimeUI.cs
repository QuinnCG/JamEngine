using Engine;
using Engine.Rendering;
using Engine.UI;

namespace Sandbox;

class TimeUI(Texture font) : UIText(font)
{
	protected override void OnUpdate()
	{
		Text = $"{Time.Now:0.00}s";
	}
}
