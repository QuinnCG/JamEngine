namespace Engine;

public static class Time
{
	public static int FrameCount { get; private set; }
	public static float Now { get; private set; }
	public static float Delta { get; private set; }

	private static float _lastTime = 0f;

	internal static void Update(float time)
	{
		FrameCount++;

		Delta = time - _lastTime;
		_lastTime = time;

		Now = time;
	}
}
