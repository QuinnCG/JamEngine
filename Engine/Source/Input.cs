using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

// TODO: Support gamepads.

public static class Input
{
	public static Vector2 MousePosition
	{
		get
		{
			unsafe
			{
				GLFW.GetCursorPos(Window.Handle, out double x, out double y);
				return new((float)x, (float)y);
			}
		}
		set
		{
			unsafe
			{
				GLFW.SetCursorPos(Window.Handle, value.X, value.Y);
			}
		}
	}
	public static float ScrollDelta { get; private set; }

	public static event Action<Key>? OnKeyPressed, OnKeyReleased;
	public static event Action<Button>? OnButtonPressed, OnButtonReleased;

	private static readonly HashSet<Key> _justPressedKeys = [], _heldKeys = [], _justReleasedKeys = [];
	private static readonly HashSet<Button> _justPressedButtons = [], _heldButtons = [], _justReleasedButtons = [];

	public static bool IsKeyPressed(Key key)
	{
		return _justPressedKeys.Contains(key);
	}
	public static bool IsKeyHeld(Key key)
	{
		return _heldKeys.Contains(key);
	}
	public static bool IsKeyReleased(Key key)
	{
		return _justReleasedKeys.Contains(key);
	}

	public static bool IsButtonPressed(Button button)
	{
		return _justPressedButtons.Contains(button);
	}
	public static bool IsButtonHeld(Button button)
	{
		return _heldButtons.Contains(button);
	}
	public static bool IsButtonReleased(Button button)
	{
		return _justReleasedButtons.Contains(button);
	}

	public static float GetAxis(Key negative, Key positive, AxisPriority priority = AxisPriority.Neither)
	{
		bool pos = IsKeyHeld(positive);
		bool neg = IsKeyHeld(negative);

		switch (priority)
		{
			case AxisPriority.Neither:
			{
				if (pos && neg)
				{
					return 0f;
				}

				if (pos)
				{
					return 1f;
				}
				else if (neg)
				{
					return -1f;
				}
				else
				{
					return 0f;
				}
			}
			case AxisPriority.Positive:
			{
				if (pos)
				{
					return 1f;
				}
				else if (neg)
				{
					return -1f;
				}
				else
				{
					return 0f;
				}
			}
			case AxisPriority.Negative:
			{
				if (neg)
				{
					return -1f;
				}
				else if (pos)
				{
					return 1f;
				}
				else
				{
					return 0f;
				}
			}
		}

		return 0f;
	}
	public static float GetAxis(Button negative, Button positive, AxisPriority priority = AxisPriority.Neither)
	{
		bool pos = IsButtonHeld(positive);
		bool neg = IsButtonHeld(negative);

		switch (priority)
		{
			case AxisPriority.Neither:
			{
				if (pos && neg)
				{
					return 0f;
				}

				if (pos)
				{
					return 1f;
				}
				else if (neg)
				{
					return -1f;
				}
				else
				{
					return 0f;
				}
			}
			case AxisPriority.Positive:
			{
				if (pos)
				{
					return 1f;
				}
				else if (neg)
				{
					return -1f;
				}
				else
				{
					return 0f;
				}
			}
			case AxisPriority.Negative:
			{
				if (neg)
				{
					return -1f;
				}
				else if (pos)
				{
					return 1f;
				}
				else
				{
					return 0f;
				}
			}
		}

		return 0f;
	}

	internal static unsafe void Initialize()
	{
		GLFW.SetKeyCallback(Window.Handle, OnKeyInput);
		GLFW.SetMouseButtonCallback(Window.Handle, OnButtonInput);
		GLFW.SetScrollCallback(Window.Handle, OnScroll);
	}

	/// <summary>
	/// Clears the input state of inputs just being pressed or released this frame.
	/// </summary>
	internal static void Clear()
	{
		_justPressedKeys.Clear();
		_justReleasedKeys.Clear();

		_justPressedButtons.Clear();
		_justReleasedButtons.Clear();

		ScrollDelta = 0f;
	}

	private static unsafe void OnKeyInput(OpenTK.Windowing.GraphicsLibraryFramework.Window* window, Keys key, int scanCode, InputAction action, KeyModifiers mods)
	{
		var k = (Key)key;

		switch (action)
		{
			case InputAction.Press:
			{
				OnKeyPressed?.Invoke(k);
				_heldKeys.Add(k);
				_justPressedKeys.Add(k);

				break;
			}
			case InputAction.Release:
			{
				OnKeyReleased?.Invoke((Key)key);
				_heldKeys.Remove(k);
				_justReleasedKeys.Add(k);

				break;
			}
		}
	}

	private static unsafe void OnButtonInput(OpenTK.Windowing.GraphicsLibraryFramework.Window* window, MouseButton button, InputAction action, KeyModifiers mods)
	{
		var b = (Button)button;

		switch (action)
		{
			case InputAction.Press:
			{
				OnButtonPressed?.Invoke((Button)button);
				_heldButtons.Add(b);
				_justPressedButtons.Add(b);

				break;
			}
			case InputAction.Release:
			{
				OnButtonReleased?.Invoke((Button)button);
				_heldButtons.Remove(b);
				_justReleasedButtons.Add(b);

				break;

			}
		}
	}

	private static unsafe void OnScroll(OpenTK.Windowing.GraphicsLibraryFramework.Window* window, double offsetX, double offsetY)
	{
		ScrollDelta = (float)offsetY;
	}
}
