using Engine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.UI;

/// <summary>
/// Base class for all UI entities.
/// </summary>
public class UIEntity : Entity
{
	// TODO: Support margins and padding?

	// TODO: Support anchors. Position and scale are relative to anchors.
	// Perhaps, anchors have fixed point mode (bot-left, top-center, etc) and fullscreen mode.

	public UIEntity? UIParent { get; private set; }

	public Sprite? Sprite { get; set; }
	public Color4 Tint { get; set; }

	public Vector2 Position
	{
		get
		{
			if (UIParent != null)
			{
				return UIParent.GetFlexPositon(this);
			}

			return _absPos;
		}
		set
		{
			_absPos = value;
		}
	}
	public Vector2 Scale
	{
		get
		{
			if (UIParent != null)
			{
				return UIParent.GetFlexScale(this);
			}

			return _absScale;
		}
		set
		{
			_absScale = value;
		}
	}

	private readonly RenderObject _renderObj;

	private Vector2 _absPos, _absScale = Vector2.One;

	public UIEntity()
		: this(Color4.Transparent) { }
	public UIEntity(Color4 tint)
	{
		Tint = tint;

		_renderObj = new RenderObject()
		{
			IsInvisible = () => Tint.A == 0f,
			OnBind = OnBind,
			OnDispose = OnDipose,

			GetIndexCount = GetIndexCount,

			IsScreenSpace = IsScreenSpace,
			GetPosition = GetPosition,
			GetRotation = GetRotation,
			GetScale = GetScale,

			GetTint = GetTint,

			GetTexture = GetTexture,
			GetUVOffset = GetUVOffset,
			GetUVScale = GetUVScale
		};
	}

	protected virtual void OnBind()
	{
		if (Tint.A > 0f)
		{
			GL.BindVertexArray(SpriteRenderer.SpriteQuadVertexArray);
			Sprite?.Texture.Bind();
		}
	}
	protected virtual void OnDipose() { }

	protected virtual int GetIndexCount() => Shape.Quad.Indices.Length;

	protected virtual bool IsScreenSpace() => true;
	protected virtual Vector2 GetPosition() => Position;
	protected virtual float GetRotation() => 0f;
	protected virtual Vector2 GetScale() => Scale;

	protected virtual Color4 GetTint() => Tint;

	protected virtual Texture? GetTexture() => Sprite?.Texture;
	protected virtual Vector2 GetUVOffset() => Sprite.HasValue ? Sprite.Value.UVOffset : Vector2.Zero;
	protected virtual Vector2 GetUVScale() => Sprite.HasValue ? Sprite.Value.UVScale : Vector2.One;

	protected override void OnCreate()
	{
		Renderer.Register(_renderObj);
	}

	protected override void OnDestroy()
	{
		Renderer.Unregister(_renderObj);
	}

	protected override void OnParentChange(Entity? oldParent, Entity? newParent)
	{
		// UNDONE: Test if this works. What happens if you pass null?
		UIParent = newParent as UIEntity;
	}

	protected virtual Vector2 GetFlexPositon(Entity child) => Position;
	protected virtual Vector2 GetFlexScale(Entity child) => Scale;

	/// <summary>
	/// Returns the center-left position.
	/// </summary>
	protected Vector2 GetLeft()
	{
		return Position - (Scale / 2f).WithY(Position.Y);
	}
	/// <summary>
	/// Returns the center-right position.
	/// </summary>
	protected Vector2 GetRight()
	{
		return Position + (Scale / 2f).WithY(Position.Y);
	}
	/// <summary>
	/// Returns the center-top position.
	/// </summary>
	protected Vector2 GetTop()
	{
		return Position + (Scale / 2f).WithX(Position.X);
	}
	/// <summary>
	/// Returns the center-bottom position.
	/// </summary>
	protected Vector2 GetBottom()
	{
		return Position - (Scale / 2f).WithX(Position.X);
	}
}
