using OpenTK.Mathematics;

namespace Engine;

public class SpatialEntity : Entity
{
	public Vector2 LocalPosition { get; set; }
	public float LocalRotation { get; set; }
	public Vector2 LocalScale { get; set; } = Vector2.One;

	public Vector2 WorldPosition
	{
		get
		{
			if (Parent is SpatialEntity ent)
			{
				return ent.WorldPosition + LocalPosition;
			}

			return LocalPosition;
		}
		set
		{
			if (Parent is SpatialEntity ent)
			{
				LocalPosition = value - ent.WorldPosition;
			}

			LocalPosition = value;
		}
	}
	public float WorldRotation
	{
		get
		{
			if (Parent is SpatialEntity ent)
			{
				return ent.WorldRotation + LocalRotation;
			}

			return LocalRotation;
		}
		set
		{
			if (Parent is SpatialEntity ent)
			{
				LocalRotation = value - ent.WorldRotation;
			}

			LocalRotation = value;
		}
	}
	public Vector2 WorldScale
	{
		get
		{
			if (Parent is SpatialEntity ent)
			{
				return ent.WorldScale + LocalScale;
			}

			return LocalScale;
		}
		set
		{
			if (Parent is SpatialEntity ent)
			{
				LocalScale = value - ent.WorldScale;
			}

			LocalScale = value;
		}
	}
}
