namespace Engine;

public enum BuildType
{
	/// <summary>
	/// The version of the game that supports swapping to an editor mode.
	/// </summary>
	Editor,
	/// <summary>
	/// A build, stripped of the editor functionality.<br/>
	/// It still has the editor's console, but know you toggle that on top of the running game, instead of swapping to edit mode.
	/// </summary>
	Development,
	/// <summary>
	/// A final version of a to-be-shipped build.<br/>
	/// This may also be used as a test when nearing a final build or for other reasons.<br/>
	/// This version of build is not meant to have any cheats of any kind.
	/// </summary>
	Release
}
