using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public class Shader : Resource
{
	public int Handle { get; private set; }

	public void Bind()
	{
		GL.UseProgram(Handle);
	}

	protected override void OnLoad(byte[] data)
	{
		string source = Encoding.Default.GetString(data);
		string[] split = source.Split("// Fragment");

		string vSrc = split[0];
		string fSrc = split[1];

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

		string info = GL.GetProgramInfoLog(Handle);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Log.Error("OpenGL", info);
		}
	}

	protected override void OnFree()
	{
		GL.DeleteProgram(Handle);
	}

	private static int CreateSubShader(ShaderType type, string source)
	{
		var shader = GL.CreateShader(type);
		GL.ShaderSource(shader, source);
		GL.CompileShader(shader);

		string info = GL.GetShaderInfoLog(shader);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Log.Error("OpenGL", info);
		}

		return shader;
	}
}
