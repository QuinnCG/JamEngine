namespace Engine;

/// <summary>
/// Uses inside <see cref="Entity"/>s and <see cref="Component"/>s as a helper for async tasks.
/// </summary>
public class Wait
{
	private CancellationTokenSource _tokenSource = new();

	internal void Cancel()
	{
		_tokenSource.Cancel();
	}

	public async Task Duration(float seconds)
	{
		await Task.Delay((int)(seconds * 1000), _tokenSource.Token);
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
			await NextFrame();
		}
	}

	public async Task EndOfFrame()
	{
		throw new NotImplementedException();
	}

	public async Task NextFrame()
	{
		throw new NotImplementedException();
	}

	// TODO: Entity and component create an internal protected instance of this.
}

// TODO: Handle issue of loading worlds.
// TODO: Test current architecture. Test everything!
