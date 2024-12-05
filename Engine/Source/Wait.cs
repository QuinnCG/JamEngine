namespace Engine;

/// <summary>
/// Used by <c>Entity</c> and <c>Component</c> to provide async functionally that respects the lifecycle of said <c>Entity</c> or <c>Component</c>.
/// </summary>
public class Wait
{
	private readonly CancellationTokenSource _cancellationSource = new();

	private bool _isNextFrame;
	private bool _isFirstUpdate;

	public Wait()
	{
		Application.OnUpdate += OnAppUpdate;
	}

	/// <summary>
	/// Waits for the specified duration.
	/// <br>This will automatically be cancelled if its parent is destroyed.</br>
	/// </summary>
	/// <param name="duration">The number of seconds to wait for. This is in scaled-time.</param>
	/// <returns>A task to <c>await</c>.</returns>
	public async Task Seconds(float duration)
	{
		float endTime = Time.Now + duration;

		await Task.Run(() =>
		{
			while (Time.Now < endTime && !_cancellationSource.IsCancellationRequested) { }
		});
	}

	internal void Destroy()
	{ 
		_cancellationSource.Cancel();
		Application.OnUpdate -= OnAppUpdate;
	}

	private void OnAppUpdate()
	{
		if (!_isFirstUpdate)
		{
			_isNextFrame = true;
		}

		_isFirstUpdate = true;
	}
}
