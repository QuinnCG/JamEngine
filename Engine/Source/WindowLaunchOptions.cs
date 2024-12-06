namespace Engine;

/// <summary>
/// Contains <c>Window</c> options that can't be set after launching.
/// </summary>
public record WindowLaunchOptions
{
	public bool CanResize { get; init; } = true;
}
