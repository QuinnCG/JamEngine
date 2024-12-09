namespace Engine.UI;

/// <summary>
/// If this is the parent of a <see cref="UIEntity"/>, that <see cref="UIEntity"/> will exist relative to this <see cref="Entity"/> and its <see cref="Transform"/>.
/// </summary>
public class WorldUI : Entity
{
	// TODO: Implement world UI.

	private readonly Transform _transform;

	public WorldUI()
	{
		_transform = CreateComponent<Transform>();
	}
}
