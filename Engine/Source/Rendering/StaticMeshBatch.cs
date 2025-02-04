using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public class StaticMeshBatch : MeshBatch
{
	/// <summary>
	/// Calling this after the batch has already been created will first call <see cref="MeshBatch.Destroy()"/>.
	/// </summary>
	public StaticMeshBatch Create(float[] vertices, uint[] indices)
	{
		if (IsCreated)
		{
			Destroy();
		}

		IsCreated = true;
		IndexCount = indices.Length;

		VAO = GL.GenVertexArray();
		GL.BindVertexArray(VAO);

		VBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

		IBO = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

		SetLayout();
		return this;
	}
}
