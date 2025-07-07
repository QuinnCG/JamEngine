using OpenTK.Mathematics;

namespace Engine.Simulation;

public static class Physics
{
	/// <summary>
	/// How many times should a physics world update per second.
	/// </summary>
	public static float StepRate { get; set; } = 60f;
	/// <summary>
	/// The fixed interval between physics world updates.
	/// </summary>
	public static float StepDelta => 1f / StepRate;

	/// <summary>
	/// Get or set the gravity of the current world.
	/// </summary>
	public static Vector2 Gravity
	{
		get
		{
			var g = World.Current.PhysicsWorld.Gravity;
			return new(g.X, g.Y);
		}

		set
		{
			World.Current.PhysicsWorld.Gravity = new(value.X, value.Y);
		}
	}
}
