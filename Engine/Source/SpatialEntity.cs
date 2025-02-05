using OpenTK.Mathematics;

namespace Engine;

public class SpatialEntity : Entity
{
	public bool ForceWorldPosition { get; set; }
	public bool ForceWorldRotation { get; set; }
	public bool ForceWorldScale { get; set; }

	public Vector2 WorldPosition
	{
		get
		{
			if (!ForceWorldPosition && Parent is SpatialEntity ent)
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
			if (!ForceWorldRotation && Parent is SpatialEntity ent)
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
			if (!ForceWorldScale && Parent is SpatialEntity ent)
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

	public Vector2 LocalPosition { get; set; }
	public float LocalRotation { get; set; }
	public Vector2 LocalScale { get; set; } = Vector2.One;

	public float LocalPositionX
	{
		get => LocalPosition.X;
		set => LocalPosition = new(value, LocalPosition.Y);
	}
	public float LocalPositionY
	{
		get => LocalPosition.Y;
		set => LocalPosition = new(LocalPosition.X, value);
	}
}
