using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

// TODO: Make this a type of Resource and use the resource's lifecycle methods.
public class Shader
{
	public int Handle { get; }

	public Shader(string vSrc, string fSrc)
	{
		Handle = GL.CreateProgram();
		Bind();

		var vs = CreateSubShader(ShaderType.VertexShader, vSrc);
		var fs = CreateSubShader(ShaderType.FragmentShader, fSrc);

		GL.AttachShader(Handle, vs);
		GL.AttachShader(Handle, fs);

		GL.LinkProgram(Handle);
		GL.ValidateProgram(Handle);

		GL.DeleteShader(vs);
		GL.DeleteShader(fs);
	}

	public void Bind()
	{
		GL.UseProgram(Handle);
	}

	public void Destroy()
	{
		GL.DeleteProgram(Handle);
	}

	private static int CreateSubShader(ShaderType type, string source)
	{
		var shader = GL.CreateShader(type);
		GL.ShaderSource(shader, source);
		GL.CompileShader(shader);

		return shader;
	}
}
