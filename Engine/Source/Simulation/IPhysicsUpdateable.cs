namespace Engine.Source.Simulation;

public interface IPhysicsUpdateable
{
	/// <summary>
	/// This is called just after the physics world is updated.<br/>
	/// This is called right before the normal <see cref="Entity.Update"/> is called.
	/// </summary>
	public void PhysicsUpdate();
}
