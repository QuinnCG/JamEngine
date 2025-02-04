using nkast.Aether.Physics2D.Common;
using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public class DynamicMeshBatch : MeshBatch
{
	// TODO: Dynamic resize.

	public void Update(float[] vertices, uint[] indices)
	{
		//// First time creation.
		//if (!IsCreated)
		//{
		//	Regenerate();
		//}
		//// Update existing.
		//else
		//{
		//	Subbuffer();
		//}

		// Temporary, until dynamic resizing is implemented.
		Regenerate(vertices, indices);
	}

	private void Regenerate(float[] vertices, uint[] indices)
	{
		IsCreated = true;
		IndexCount = indices.Length;

		VAO = GL.GenVertexArray();
		GL.BindVertexArray(VAO);

		VBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

		IBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.DynamicDraw);

		SetLayout();
	}

	//private void Subbuffer()
	//{

	//}
}
