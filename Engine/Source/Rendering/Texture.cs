using OpenTK.Graphics.OpenGL4;
using StbiSharp;

namespace Engine.Rendering;

public class Texture : Resource, IBindable
{
	public int ColorChannelCount { get; private set; }

	/// <summary>
	/// Should the image be anti-aliased or not. <br/>
	/// Pixel art should have this set to <c>false</c>, but other images such as branding images may want this <c>true</c>.
	/// </summary>
	/// <remarks>This is <c>false</c> by default.</remarks>
	public bool IsFiltered
	{
		get => _isFiltered;
		set
		{
			if (_isFiltered != value)
			{
				_isFiltered = value;
				Bind();

				if (_isFiltered)
				{
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
				}
				else
				{
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
				}
			}
		}
	}
	/// <summary>
	/// Is the image to be repeated when its UV is made smaller than the quad.
	/// </summary>
	/// <remarks>This is <c>true</c> by default.</remarks>
	public bool IsRepeated
	{
		get => _isRepeated;
		set
		{
			if (_isRepeated != value)
			{
				_isRepeated = value;

				if (_isRepeated)
				{
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
				}
				else
				{
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
					GL.SamplerParameter(_sampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
				}
			}
		}
	}

	private int _texture;
	private int _sampler;

	private bool _isFiltered;
	private bool _isRepeated = true;

	protected override void OnLoad(byte[] data)
	{
		var span = new ReadOnlySpan<byte>(data);

		Stbi.SetFlipVerticallyOnLoad(true);
		var image = Stbi.LoadFromMemory(span, 4);

		ColorChannelCount = image.NumChannels;

		_sampler = GL.GenSampler();
		_texture = GL.GenTexture();
		Bind();

		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data.ToArray());
		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

		GL.SamplerParameter(_sampler, SamplerParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
		GL.SamplerParameter(_sampler, SamplerParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
		GL.SamplerParameter(_sampler, SamplerParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.SamplerParameter(_sampler, SamplerParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
	}

	public void Bind()
	{
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, _texture);

		GL.BindSampler(0, _sampler);
	}

	protected override void OnFree()
	{
		GL.DeleteTexture(_texture);
	}
}
