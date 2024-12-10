namespace Engine;

/// <summary>
/// Inherit from this class to create groups of <see cref="Entity"/> that will be instantiated together.
/// </summary>
public abstract class EntityPrefab : Entity
{
	protected override void OnCreate()
	{
		foreach (var entity in SpawnEntities())
		{
			World.AddEntity(entity);

			// TODO: Test EntityPrefab.
			// Make sure child entities get proper references to World.
		}
	}

	protected sealed override void OnUpdate() { }

	protected sealed override void OnFixedUpdate() { }

	protected override void OnDestroy()
	{

	}

	public abstract IEnumerable<Entity> SpawnEntities();
}
