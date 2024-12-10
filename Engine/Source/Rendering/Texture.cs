using Engine.Resources;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Rendering;

public class Texture : RenderElement
{
	public Vector2i Size { get; }

	private readonly int _handle;

	public Texture(TextureResource res)
	{
		_handle = GL.GenTexture();
		Bind();

		Size = res.Size;

		GL.TextureParameter(_handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
		GL.TextureParameter(_handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
		GL.TextureParameter(_handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		GL.TextureParameter(_handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, res.Size.X, res.Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, res.Pixels);
		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
	}
	~Texture()
	{
		OnDipose();
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
