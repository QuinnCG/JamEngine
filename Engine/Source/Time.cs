namespace Engine;

public static class Time
{
	public static int FixedStepDivision
	{
		get => _fixedStepDivision;
		set
		{
			_fixedStepDivision = value;
			FixedDelta = 1f / _fixedStepDivision;
		}
	}

	public static float Now { get; private set; }
	public static float Delta { get; private set; }
	public static float FixedDelta { get; private set; }
	public static int Ticks { get; private set; }

	private static int _fixedStepDivision;
	private static float _lastTime;

	// TODO: Make time scaled and add unscaled variants.

	internal static void Update(float now)
	{
		Now = now;
		Delta = now - _lastTime;

		_lastTime = now;
		Ticks++;
	}
}
