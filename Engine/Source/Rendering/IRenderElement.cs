namespace Engine.Rendering;

public abstract class RenderElement : IDisposable
{
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
