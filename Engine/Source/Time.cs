namespace Engine;

public static class Time
{
	/// <summary>
	/// The number of frames that have been rendered since the start of the game.<br/>
	/// This will be 0 for the first frame.
	/// </summary>
	public static int FrameCount { get; private set; }
	/// <summary>
	/// The current scaled time of the game, in seconds.
	/// </summary>
	public static float Now { get; private set; }
	/// <summary>
	/// The time, in seconds, that the last frame took to render.<br/>
	/// Note: this will be 0s for the first frame.
	/// </summary>
	public static float Delta { get; private set; }
	public static float Scale { get; set; } = 1f;

	private static float _lastTime = 0f;

	internal static void Update(float time)
	{
		FrameCount++;

		Delta = (time - _lastTime) * Scale;
		_lastTime = time;

		Now += Delta;
	}
}
