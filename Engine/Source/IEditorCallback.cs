namespace Engine;

/// <summary>
/// Implement on <see cref="Entity"/>s or <see cref="Component"/>s to receive callbacks while in edit-mode on those objects when they are in a loaded <see cref="EditorWorld"/>.
/// </summary>
public interface IEditorCallback
{
	public void EditorCreate();
	public void EditorDestroy();
}
