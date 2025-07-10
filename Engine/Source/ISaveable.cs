namespace Engine;

/// <summary>
/// Implemented by <see cref="Resource"/> classes that want to be able to serialize their states to disk.
/// </summary>
public interface ISaveable
{
	/// <summary>
	/// Use the resource's custom save implementation to serialize the resource to disk.
	/// </summary>
	public void Save();
}
