using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GLFWWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace Engine;

public static unsafe class Window
{
	public static string Title
	{
		get => _title;
		set
		{
			_title = value;

			if (Handle != null)
				GLFW.SetWindowTitle(Handle, value);
		}
	}
	public static Vector2i Size
	{
		get => _size;
		set
		{
			_size = value;

			if (Handle != null)
				GLFW.SetWindowSize(Handle, value.X, value.Y);
		}
	}

	public static float AspectRatio => (float)Size.X / Size.Y;
	public static WindowLaunchOptions LaunchOptions => _launchOptions!;

	public static event Action<Vector2i>? OnWindowResize;

	internal static bool ShouldClose => GLFW.WindowShouldClose(Handle);

	internal static event Action<Keys>? OnKeyPressed, OnKeyReleased;
	internal static event Action<MouseButton>? OnButtonPressed, OnButtonReleased;
	internal static event Action<float>? OnScroll;

	internal static GLFWWindow* Handle { get; private set; }

	private static WindowLaunchOptions? _launchOptions;
	private static string _title = "Jam Engine";
	private static Vector2i _size = new(1440, 800);

	// TODO: Cursor hiding/locking.
	//GLFW.SetInputMode(_handle, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);

	internal static void Launch(WindowLaunchOptions options)
	{
		_launchOptions = options;

		GLFW.Init();

		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		GLFW.WindowHint(WindowHintBool.Resizable, options.CanResize);
		
		if (options.EnableVSync)
		{
			GLFW.SwapInterval(1);
		}

		Handle = GLFW.CreateWindow(_size.X, _size.Y, _title, null, null);
		GLFW.MakeContextCurrent(Handle);

		GLFW.SetFramebufferSizeCallback(Handle, OnFrameBufferSizeChange);

		GLFW.SetKeyCallback(Handle, OnKeyStateChange);
		GLFW.SetMouseButtonCallback(Handle, OnButtonStateChange);
		GLFW.SetScrollCallback(Handle, OnScrollDelta);
	}

	internal static Vector2 GetMousePosition()
	{
		GLFW.GetCursorPos(Handle, out double x, out double y);
		return new((float)x, (float)y);
	}

	internal static void SetMousePosition(Vector2 pos)
	{
		GLFW.SetCursorPos(Handle, pos.X, pos.Y);
	}

	private static void OnKeyStateChange(GLFWWindow* window, Keys key, int scanCode, InputAction action, KeyModifiers mods)
	{
		if (action == InputAction.Press)
		{
			OnKeyPressed?.Invoke(key);
		}
		else if (action == InputAction.Release)
		{
			OnKeyReleased?.Invoke(key);
		}
	}

	private static void OnButtonStateChange(GLFWWindow* window, MouseButton button, InputAction action, KeyModifiers mods)
	{
		if (action == InputAction.Press)
		{
			OnButtonPressed?.Invoke(button);
		}
		else if (action == InputAction.Release)
		{
			OnButtonReleased?.Invoke(button);
		}
	}

	private static void OnScrollDelta(GLFWWindow* window, double x, double y)
	{
		OnScroll?.Invoke((float)y);
	}

	internal static void Close()
	{
		GLFW.SetWindowShouldClose(Handle, true);
	}

	internal static void PollEvents()
	{
		GLFW.PollEvents();
	}

	internal static void SwapBuffers()
	{
		GLFW.SwapBuffers(Handle);
	}

	internal static void CleanUp()
	{
		GLFW.DestroyWindow(Handle);
		GLFW.Terminate();

		Handle = null;
	}

	private static void OnFrameBufferSizeChange(GLFWWindow* window, int width, int height)
	{
		Size = new(width, height);
		OnWindowResize?.Invoke(Size);
	}
}
