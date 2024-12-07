using OpenTK.Graphics.OpenGL4;
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

			if (_handle != null)
				GLFW.SetWindowTitle(_handle, value);
		}
	}
	public static Vector2i Size
	{
		get => _size;
		set
		{
			_size = value;

			if (_handle != null)
				GLFW.SetWindowSize(_handle, value.X, value.Y);
		}
	}

	public static float AspectRatio => (float)Size.X / Size.Y;
	public static WindowLaunchOptions LaunchOptions => _launchOptions!;

	public static event Action<Vector2i>? OnWindowResize;

	internal static bool ShouldClose => GLFW.WindowShouldClose(_handle);

	internal static event Action<Keys>? OnKeyPressed, OnKeyReleased;
	internal static event Action<MouseButton>? OnButtonPressed, OnButtonReleased;
	internal static event Action<float>? OnScroll;

	private static GLFWWindow* _handle;
	private static WindowLaunchOptions? _launchOptions;
	private static string _title = "Jam Engine";
	private static Vector2i _size = new(1440, 800);

	internal static void Launch(WindowLaunchOptions options)
	{
		_launchOptions = options;

		GLFW.Init();

		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		GLFW.WindowHint(WindowHintBool.Resizable, options.CanResize);

		_handle = GLFW.CreateWindow(_size.X, _size.Y, _title, null, null);
		GLFW.MakeContextCurrent(_handle);

		GLFW.SetFramebufferSizeCallback(_handle, OnFrameBufferSizeChange);

		GLFW.SetKeyCallback(_handle, OnKeyStateChange);
		GLFW.SetMouseButtonCallback(_handle, OnButtonStateChange);
		GLFW.SetScrollCallback(_handle, OnScrollDelta);
	}

	internal static Vector2 GetMousePosition()
	{
		GLFW.GetCursorPos(_handle, out double x, out double y);
		return new((float)x, (float)y);
	}

	internal static void SetMousePosition(Vector2 pos)
	{
		GLFW.SetCursorPos(_handle, pos.X, pos.Y);
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
		GLFW.SetWindowShouldClose(_handle, true);
	}

	internal static void PollEvents()
	{
		GLFW.PollEvents();
	}

	internal static void SwapBuffers()
	{
		GLFW.SwapBuffers(_handle);
	}

	internal static void CleanUp()
	{
		GLFW.DestroyWindow(_handle);
		GLFW.Terminate();

		_handle = null;
	}

	private static void OnFrameBufferSizeChange(GLFWWindow* window, int width, int height)
	{
		Size = new(width, height);
		OnWindowResize?.Invoke(Size);
	}
}
