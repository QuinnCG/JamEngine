using OpenTK.Mathematics;

namespace Engine;

public class SpatialEntity : Entity
{
	public Vector2 LocalPosition { get; set; }
	public float LocalRotation { get; set; }
	public Vector2 LocalScale { get; set; } = Vector2.One;

	public Vector2 Position
	{
		get
		{
			if (Parent is SpatialEntity ent)
			{
				return ent.Position + LocalPosition;
			}

			return LocalPosition;
		}
		set
		{
			if (Parent is SpatialEntity ent)
			{
				LocalPosition = value - ent.Position;
			}

			LocalPosition = value;
		}
	}
	public float Rotation
	{
		get
		{
			if (Parent is SpatialEntity ent)
			{
				return ent.Rotation + LocalRotation;
			}

			return LocalRotation;
		}
		set
		{
			if (Parent is SpatialEntity ent)
			{
				LocalRotation = value - ent.Rotation;
			}

			LocalRotation = value;
		}
	}
	public Vector2 Scale
	{
		get
		{
			if (Parent is SpatialEntity ent)
			{
				return ent.Scale + LocalScale;
			}

			return LocalScale;
		}
		set
		{
			if (Parent is SpatialEntity ent)
			{
				LocalScale = value - ent.Scale;
			}

			LocalScale = value;
		}
	}
}
