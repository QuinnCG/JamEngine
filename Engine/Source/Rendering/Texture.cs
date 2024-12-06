using Engine.Resources;
using OpenTK.Graphics.OpenGL4;

namespace Engine.Rendering;

public class Texture : RenderElement
{
	private readonly int _handle;

	/// <summary>
	/// The <c>TextureResource</c> must be disposed of manually.
	/// </summary>
	public Texture(TextureResource res)
	{
		_handle = GL.GenTexture();
		Bind();

		GL.TextureParameter(_handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
		GL.TextureParameter(_handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
		GL.TextureParameter(_handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		GL.TextureParameter(_handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, res.Size.X, res.Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, res.Pixels);
		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
	}

	protected override void OnBind()
	{
		GL.BindTexture(TextureTarget.Texture2D, _handle);
	}

	protected override void OnDipose()
	{
		GL.DeleteTexture(_handle);
	}
}
