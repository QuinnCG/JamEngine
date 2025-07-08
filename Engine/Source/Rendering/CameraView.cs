using OpenTK.Mathematics;

namespace Engine.Rendering;

public class CameraView : Component
{
	public static CameraView Current { get; private set; }

	public float OrthographicSize { get; set; } = 5f;

	public Matrix4 ViewMatrix => GetViewMatrix();
	public Matrix4 ProjectionMatrix => GetProjectionMatrix();
	public Matrix4 ViewProjectionMatrix => ViewMatrix * ProjectionMatrix;

	public override string ToString()
	{
		return base.ToString() + $" <OrthoSize: {OrthographicSize}>";
	}

	public Vector2 ScreenToWorldPoint(Vector2 screenPoint)
	{
		// Convert to a point between -1 and +1.
		Vector2 point = screenPoint / Window.Resolution;
		// Make y-up positive.
		point.Y = 1f - point.Y;

		point *= 2f;
		point -= Vector2.One;

		// Apply camera transform.
		var transformed = new Vector4(point.X, point.Y, 0f, 1f) * GetReverseViewMatrix() * GetProjectionMatrix();

		return new(transformed.X, transformed.Y);
	}

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

	/// <summary>
	/// Normally, if the camera goes right, everything goes left, but here, if the camera goes right, then the screen-world point will always go right.
	/// </summary>
	private Matrix4 GetReverseViewMatrix()
	{
		return Matrix4.CreateTranslation(Entity.Position.X, Entity.Position.Y, 0f) *
			   Matrix4.CreateRotationZ(-Entity.Rotation * MathX.DegToRad) *
			   Matrix4.CreateScale(Entity.Scale.X, Entity.Scale.Y, 1f);
	}

	private Matrix4 GetProjectionMatrix()
	{
		return Matrix4.CreateOrthographic((float)Window.Resolution.X / Window.Resolution.Y * OrthographicSize, OrthographicSize, 0f, 1f);
	}
}
