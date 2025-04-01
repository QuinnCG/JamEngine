namespace Engine;

public class SpatialEntity : Entity
{
	public Transform Transform { get; }

	public SpatialEntity()
	{
		Transform = CreateComponent<Transform>();
	}
}
