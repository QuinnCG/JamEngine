using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public abstract class MeshBatch
{
	public bool IsCreated { get; protected set; }
	public int IndexCount { get; protected set; }

	protected int VAO { get; set; }
	protected int VBO { get; set; }
	protected int IBO { get; set; }

	protected int[] LayoutComponentCounts { get; private set; }

	/// <summary>
	/// The default layout will be [2, 2].<br/>
	/// 2 floats for the vertex position followed by two floats for the UV coords.
	/// </summary>
	public MeshBatch()
	{
		LayoutComponentCounts = [2, 2];
	}
	/// <summary>
	/// Stipulate a custom layout other than the default provided by the parameterless constructor.<br/>
	/// You will need to manually describe basics such as position and UVs in addition to whatever else you wish to add.
	/// </summary>
	/// <param name="layoutComponentCounts">Each item is a component whose numerical value is the number of floats within that component.</param>
	public MeshBatch(params int[] layoutComponentCounts)
	{
		LayoutComponentCounts = layoutComponentCounts;
	}

	public void Bind()
	{
		if (IsCreated)
		{
			GL.BindVertexArray(VAO);
		}
	}

	public void Destroy()
	{
		if (IsCreated)
		{
			IsCreated = false;

			GL.DeleteVertexArray(VAO);
			GL.DeleteBuffer(VBO);
			GL.DeleteBuffer(IBO);
		}
	}

	protected void SetLayout()
	{
		int stride = sizeof(float) * LayoutComponentCounts.Sum();
		int compCounter = 0;

		for (int i = 0; i < LayoutComponentCounts.Length; i++)
		{
			int compCount = LayoutComponentCounts[i];

			GL.EnableVertexAttribArray(i);
			GL.VertexAttribPointer(i, compCount, VertexAttribPointerType.Float, false, stride, sizeof(float) * compCounter);

			compCounter += compCount;
		}
	}
}
