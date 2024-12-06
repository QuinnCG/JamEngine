namespace Engine.Rendering;

// TODO: Reference count render elements and dipose automatically?
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
