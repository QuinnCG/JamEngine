using OpenTK.Mathematics;

namespace Engine.UI;

public struct UIRect(Vector2 center, Vector2 size)
{
	public Vector2 Center = center;
	public Vector2 Size = size;
}
