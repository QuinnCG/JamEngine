using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public class Mesh : RenderElement
{
	public int IndexCount { get; }

	internal int VAO { get; set; }
	internal int VBO { get; set; }
	internal int IBO { get; set; }

	private bool _isStatic;

	public Mesh(Vertex[] vertices, uint[] indices, bool isStatic = true)
	{
		_isStatic = isStatic;

		float[] vertexData = Vertex.GetRaw(vertices);
		var usage = isStatic ? BufferUsageHint.StaticDraw : BufferUsageHint.DynamicDraw;

		VAO = GL.GenVertexArray();
		GL.BindVertexArray(VAO);

		VBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertexData.Length, vertexData, usage);

		IBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, usage);

		Vertex.SetGLLayout();
		GL.BindVertexArray(0);

		IndexCount = indices.Length;
	}

	public void Update(Vertex[] vertices, uint[] indices)
	{
		Log.Assert(!_isStatic, $"Cannot update Mesh because it is static!");

		// TODO: Support updating mesh.
		// Perhaps, have method to update existing buffer and one that updates but can also auto grow.
		// For latter, maybe bool to control if its allowed to shrink if possible.
		throw new NotImplementedException();
	}

	protected override void OnBind()
	{
		GL.BindVertexArray(VAO);
	}

	protected override void OnDipose()
	{
		GL.DeleteVertexArray(VAO);
		GL.DeleteBuffer(VBO);
		GL.DeleteBuffer(IBO);

		GL.BindVertexArray(0);
	}
}
