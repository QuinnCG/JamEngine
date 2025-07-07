namespace Engine.Simulation;

public enum RigidbodyType
{
	/// <summary>
	/// Positive mass, non-zero velocity determined by forces, moved by solver.
	/// </summary>
	Dynamic = 2,
	/// <summary>
	/// Zero velocity, may be manually moved. Note: even static bodies have mass.
	/// </summary>
	Static = 0,
	/// <summary>
	/// Zero mass, non-zero velocity set by user, moved by solver.
	/// </summary>
	Kinematic = 1,
}
