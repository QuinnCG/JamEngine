using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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

		var vs = CreateModule(ShaderType.VertexShader, vSrc);
		var fs = CreateModule(ShaderType.FragmentShader, fSrc);

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
		else
		{
			Bind();
		}
	}

	protected override void OnFree()
	{
		GL.DeleteProgram(Handle);
		Handle = -1;
	}

	private static int CreateModule(ShaderType type, string source)
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

	public void SetUniform(string name, int value)
	{
		GL.Uniform1(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, uint value)
	{
		GL.Uniform1(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, float value)
	{
		GL.Uniform1(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, double value)
	{
		GL.Uniform1(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, bool value)
	{
		GL.Uniform1(GetUniformLoc(name), value ? 1 : 0);
	}
	public void SetUniform(string name, int[] value)
	{
		GL.Uniform1(GetUniformLoc(name), value.Length, value);
	}
	public void SetUniform(string name, uint[] value)
	{
		GL.Uniform1(GetUniformLoc(name), value.Length, value);
	}
	public void SetUniform(string name, float[] value)
	{
		GL.Uniform1(GetUniformLoc(name), value.Length, value);
	}
	public void SetUniform(string name, double[] value)
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
	public void SetUniform(string name, Vector2i value)
	{
		GL.Uniform2(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Vector3i value)
	{
		GL.Uniform3(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Vector4i value)
	{
		GL.Uniform4(GetUniformLoc(name), value);
	}
	public void SetUniform(string name, Matrix4 value)
	{
		GL.UniformMatrix4(GetUniformLoc(name), true, ref value);
	}
	public void SetUniform(string name, Color4 value)
	{
		GL.Uniform4(GetUniformLoc(name), value);
	}

	private int GetUniformLoc(string name)
	{
		return GL.GetUniformLocation(Handle, name);
	}
}
