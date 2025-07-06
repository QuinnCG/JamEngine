using OpenTK.Mathematics;

namespace Engine.Rendering;

public class CameraView : Component
{
	public static CameraView Current { get; private set; }

	public float OrthographicSize { get; set; } = 5f;

	public Matrix4 ViewMatrix => GetViewMatrix();
	public Matrix4 ProjectionMatrix => GetProjectionMatrix();
	public Matrix4 ViewProjectionMatrix => ViewMatrix * ProjectionMatrix;

	protected override void OnCreate()
	{
		Current ??= this;
	}

	private Matrix4 GetViewMatrix()
	{
		return Matrix4.CreateTranslation(-Entity.Position.X, -Entity.Position.Y, 0f) *
			   Matrix4.CreateRotationZ(Entity.Rotation * MathX.DegToRad) *
			   Matrix4.CreateScale(Entity.Scale.X, Entity.Scale.Y, 1f);
	}

	private Matrix4 GetProjectionMatrix()
	{
		return Matrix4.CreateOrthographic((float)Window.Resolution.X / Window.Resolution.Y * OrthographicSize, OrthographicSize, 0f, 1f);
	}
}
