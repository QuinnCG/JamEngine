using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

internal class Shader : RenderElement
{
	private readonly int _handle;

	public Shader(string source)
		: this(source[..source.IndexOf("// Fragment")],
			  source[source.IndexOf("// Fragment")..]) { }
	public Shader(string vertexSource, string fragmentSource)
	{
		int vs = CreateShader(ShaderType.VertexShader, vertexSource);
		int fs = CreateShader(ShaderType.FragmentShader, fragmentSource);

		_handle = GL.CreateProgram();
		GL.AttachShader(_handle, vs);
		GL.AttachShader(_handle, fs);
		GL.LinkProgram(_handle);
		GL.ValidateProgram(_handle);

		string info = GL.GetProgramInfoLog(_handle);
		if (!string.IsNullOrWhiteSpace(info))
		{
			Log.Error(Renderer.DebugLogCategory, info);
			Log.Break();
		}

		GL.DeleteShader(vs);
		GL.DeleteShader(fs);

		Bind();
	}

	protected override void OnBind()
	{
		GL.UseProgram(_handle);
	}

	protected override void OnDipose()
	{
		GL.DeleteProgram(_handle);
	}

	public void SetUniform(string name, bool value)
	{
		GL.Uniform1(GetUnfiromLoc(name), Convert.ToInt32(value));
	}
	public void SetUniform(string name, Color4 color)
	{
		GL.Uniform4(GetUnfiromLoc(name), color);
	}
	public void SetUniform(string name, Matrix4 matrix)
	{
		GL.UniformMatrix4(GetUnfiromLoc(name), true, ref matrix);
	}
	public void SetUniform(string name, Vector2 vector)
	{
		GL.Uniform2(GetUnfiromLoc(name), vector);
	}

	private int GetUnfiromLoc(string name)
	{
		return GL.GetUniformLocation(_handle, name);
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
