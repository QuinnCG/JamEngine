using Engine.Rendering;

namespace Engine.UI;

public class UIEntity : Entity
{
	// Supports a UI transform.
	// TODO: How do we handle the top-most level? A special canvas entity?

	// TODO: Need concept of being focused, such as mouse is hovering over and not being covered; does this require a physics raycast or can we fake it?
	// Mayne no concept of focus, just interface callbacks for mouse events such as enter and exit. To check for events, check if mouse is within any rects for UI in front of target.

	// TODO: First things first make some kind of batch object for managing batches and have it support static or dynamic and have methods for building quads.
	// Maybe mesh builder is separate class where you call methods (such as Quad()) then by the end call build to get get the mesh returned which contains vertices and indices.
	// But then what data does a vertex contain? Maybe forgo the mesh object idea and have the mesh builder allow settings for tacking on additional data per vertex:
	//		by default it generates positional data, with an option enabled UV data too, and you can then also in the quad method input params float[] customData.

	public RenderLayer Layer { get; set; } = RenderLayer.Default;
}
