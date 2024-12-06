using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine;

public static class Input
{
	public static Vector2 MousePosition
	{
		get => Window.GetMousePosition();
		set => Window.SetMousePosition(value);
	}

	private static readonly HashSet<Keys> _pressedKeys = [];
	private static readonly HashSet<Keys> _heldKeys = [];
	private static readonly HashSet<Keys> _releasedKeys = [];

	private static readonly HashSet<MouseButton> _pressedButtons = [];
	private static readonly HashSet<MouseButton> _heldButtons = [];
	private static readonly HashSet<MouseButton> _releasedButtons = [];

	public static bool IsPressed(Key key)
	{
		return _pressedKeys.Contains((Keys)key);
	}
	public static bool IsPressed(Button button)
	{
		return _pressedButtons.Contains((MouseButton)button);
	}

	public static bool IsHeld(Key key)
	{
		return _heldKeys.Contains((Keys)key);
	}
	public static bool IsHeld(Button button)
	{
		return _heldButtons.Contains((MouseButton)button);
	}

	public static bool IsReleased(Key key)
	{
		return _releasedKeys.Contains((Keys)key);
	}
	public static bool IsReleased(Button button)
	{
		return _releasedButtons.Contains((MouseButton)button);
	}

	public static float GetAxis(Key negative, Key positive, InputAxisWin win = InputAxisWin.Positive)
	{
		switch (win)
		{
			case InputAxisWin.Positive:
			{
				if (IsHeld(positive))
					return 1f;
				else if (IsHeld(negative))
					return -1f;

				break;
			}
			case InputAxisWin.Negative:
			{
				if (IsHeld(negative))
					return -1f;
				else if (IsHeld(positive))
					return 1f;

				break;
			}
		}

		return 0f;
	}

	internal static void Initialize()
	{
		Window.OnKeyPressed += OnKeyPressed;
		Window.OnKeyReleased += OnKeyReleased;

		Window.OnButtonPressed += OnButtonPressed;
		Window.OnButtonReleased += OnButtonReleased;
	}

	internal static void Reset()
	{
		_pressedKeys.Clear();
		_releasedKeys.Clear();

		_pressedButtons.Clear();
		_releasedButtons.Clear();
	}

	private static void OnKeyPressed(Keys key)
	{
		_pressedKeys.Add(key);
		_heldKeys.Add(key);
	}

	private static void OnKeyReleased(Keys key)
	{
		_releasedKeys.Add(key);
		_heldKeys.Remove(key);
	}

	private static void OnButtonPressed(MouseButton button)
	{
		_pressedButtons.Add(button);
		_heldButtons.Add(button);
	}

	private static void OnButtonReleased(MouseButton button)
	{
		_heldButtons.Remove(button);
		_releasedButtons.Remove(button);
	}
}
