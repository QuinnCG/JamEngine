using OpenTK.Mathematics;

namespace Engine;

public class Transform : Component, ITransform
{
	// TODO: Support position relative to parent.
	// Have local transform parts (pos, rot, scale) as well.
	// Allow opting out of relative transform on a per data basis (pos, rot, scale).

	public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public float Rotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public Vector2 Scale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
