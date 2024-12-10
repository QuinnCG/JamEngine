namespace Engine;

public interface IResource : IDisposable
{
	public void Load(byte[] data);
	public bool IsLoaded() => true;
}
