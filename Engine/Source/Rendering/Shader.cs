using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

internal class Shader : IRenderElement
{
	public int Handle { get; }

	public Shader(string source)
		: this(source[..source.IndexOf("// Fragment")],
			  source[source.IndexOf("// Fragment")..]) { }
	public Shader(string vertexSource, string fragmentSource)
	{
		int vs = CreateShader(ShaderType.VertexShader, vertexSource);
		int fs = CreateShader(ShaderType.FragmentShader, fragmentSource);

		Handle = GL.CreateProgram();
		GL.AttachShader(Handle, vs);
		GL.AttachShader(Handle, fs);
		GL.LinkProgram(Handle);
		GL.ValidateProgram(Handle);

		string info = GL.GetProgramInfoLog(Handle);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Log.Error(Renderer.DebugLogCategory, info);
			Log.Break();
		}

		GL.DeleteShader(vs);
		GL.DeleteShader(fs);

		Bind();
	}

	public void Bind()
	{
		GL.UseProgram(Handle);
	}

	public void Dispose()
	{
		GL.DeleteProgram(Handle);
	}

	// TODO: Uniforms.

	public void SetUniform(string name, Color4 color)
	{
		GL.Uniform4(GetUnfiromLoc(name), color);
	}

	private int GetUnfiromLoc(string name)
	{
		return GL.GetUniformLocation(Handle, name);
	}

	private static int CreateShader(ShaderType type, string source)
	{
		int shader = GL.CreateShader(type);
		GL.ShaderSource(shader, source);
		GL.CompileShader(shader);

		string info = GL.GetShaderInfoLog(shader);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Log.Error(Renderer.DebugLogCategory, info);
			Log.Break();
		}

		return shader;
	}
}
