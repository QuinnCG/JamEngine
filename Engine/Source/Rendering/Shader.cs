using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Text;

namespace Engine.Rendering;

public class Shader : Resource
{
	public const string LogCategory = "Shader";

	private int _handle;

	protected override void OnLoad(byte[] data)
	{
		string source = Encoding.Default.GetString(data);
		int splitIndex = source.IndexOf("// Fragment");

		string vSrc = source[..splitIndex];
		string fSrc = source[splitIndex..];

		_handle = GL.CreateProgram();
		GL.AttachShader(_handle, CreatePass(ShaderType.VertexShader, vSrc));
		GL.AttachShader(_handle, CreatePass(ShaderType.FragmentShader, fSrc));
		GL.LinkProgram(_handle);
		GL.ValidateProgram(_handle);

		string log = GL.GetProgramInfoLog(_handle);
		if (!string.IsNullOrWhiteSpace(log))
		{
			Log.Error(LogCategory, $"Failed to create shader!\n{log}");
		}
	}

	public void Bind()
	{
		GL.UseProgram(_handle);
	}

	public void SetUniform(string name, int value)
	{
		GL.Uniform1(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, float value)
	{
		GL.Uniform1(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, bool value)
	{
		GL.Uniform1(GetUniformLoc(name), Convert.ToInt32(value));
	}
	public void SetUniform(string name, int[] value)
	{
		GL.Uniform1(GetUniformLoc(name), value.Length, value);
	}
	public void SetUniform(string name, float[] value)
	{
		GL.Uniform1(GetUniformLoc(name), value.Length, value);
	}
	public void SetUniform(string name, Vector2 value)
	{
		GL.Uniform2(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Vector3 value)
	{
		GL.Uniform3(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Vector4 value)
	{
		GL.Uniform4(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Color4 value)
	{
		GL.Uniform4(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Matrix3x2 value)
	{
		GL.UniformMatrix3x2(GetUniformLoc(name), true, ref value);
	}
	public void SetUniform(string name, Matrix4 value)
	{
		GL.UniformMatrix4(GetUniformLoc(name), true, ref value);
	}

	private int GetUniformLoc(string name)
	{
		return GL.GetUniformLocation(_handle, name);
	}

	protected override void OnFree()
	{
		GL.DeleteProgram(_handle);
	}

	private static int CreatePass(ShaderType type, string source)
	{
		int handle = GL.CreateShader(type);
		GL.ShaderSource(handle, source);
		GL.CompileShader(handle);

		string log = GL.GetShaderInfoLog(handle);
		if (!string.IsNullOrWhiteSpace(log))
		{
			Log.Error(LogCategory, $"Failed to create shader pass of type '{type}'!\n{log}");
		}

		return handle;
	}
}
