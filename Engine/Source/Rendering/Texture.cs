using OpenTK.Graphics.OpenGL4;
using StbiSharp;

namespace Engine.Rendering;

/// <summary>
/// An image that can be used for texturing meshes.<br/>
/// This exists on the GPU as a texture resource. The underlying pixel data of the image is removed from the CPU once the GPU representation is created.
/// </summary>
public class Texture : Resource
{
	public int Handle { get; private set; }

	/// <summary>
	/// The width of the texture, in pixels, or -1, if the image hasn't loaded.
	/// </summary>
	public int Width { get; private set; } = -1;
	/// <summary>
	/// The height of the texture in pixels, or -1, if the image hasn't loaded.
	/// </summary>
	public int Height { get; private set; } = -1;
	/// <summary>
	/// A value of 4 would mean an R, G, B, and A channel exists; not necessarily in that order.
	/// </summary>
	public int ChannelCount { get; private set; } = -1;

	public void Bind()
	{
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, Handle);
	}

	// TODO: [Texture.cs] Add option for antialiasing the texture.

	protected override void OnLoad(byte[] data)
	{
		Stbi.SetFlipVerticallyOnLoad(true);

		using var memory = new MemoryStream(data, 0, data.Length, false, true);
		var image = Stbi.LoadFromMemory(memory, 4);

		Width = image.Width;
		Height = image.Height;
		ChannelCount = image.NumChannels;

		Handle = GL.GenTexture();
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, Handle);

		GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
		GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
		GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
		GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);

		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data.ToArray());
		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
	}

	protected override void OnFree()
	{
		GL.DeleteTexture(Handle);
		Handle = -1;
	}
}
