namespace Engine;

/// <summary>
/// Uses inside <see cref="Entity"/>s and <see cref="Component"/>s as a helper for async tasks.
/// </summary>
public class Wait
{
	private readonly CancellationTokenSource _tokenSource = new();

	internal void Cancel()
	{
		_tokenSource.Cancel();
	}

	public async Task While(Func<bool> predicate)
	{
		while (predicate() && !_tokenSource.Token.IsCancellationRequested)
		{
			await NextFrame();
		}
	}

	public async Task Until(Func<bool> predicate)
	{
		while (!predicate() && !_tokenSource.Token.IsCancellationRequested)
		{
			await Task.Yield();
		}
	}

	public async Task Duration(float seconds)
	{
		float endTime = Time.Now + seconds;
		await Until(() => Time.Now >= endTime);
	}

	public async Task EndOfFrame()
	{
		await Until(() => Application.IsEndOfFrame);
	}

	public async Task NextFrame()
	{
		int frame = Time.FrameCount;
		await Until(() => Time.FrameCount > frame);
	}
}
