namespace Engine.Rendering;

/// <summary>
/// <see cref="Dispose"/> is only needed for disposing at specific times.
/// <br>If not called, a finalizer will dispose automatically when the <see cref="GC"/> collects.</br>
/// </summary>
public abstract class RenderElement : IDisposable
{
	~RenderElement()
	{
		OnDipose();
	}

	internal void Bind()
	{
		OnBind();
	}
	public void Dispose()
	{
		OnDipose();
		GC.SuppressFinalize(this);
	}

	protected abstract void OnBind();
	protected abstract void OnDipose();
}
