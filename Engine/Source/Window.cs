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

	internal static bool ShouldClose => GLFW.WindowShouldClose(_handle);

	private static GLFWWindow* _handle;
	private static string _title = "Jam Engine";
	private static Vector2i _size = new(1440, 800);

	internal static void Launch()
	{
		GLFW.Init();

		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		GLFW.WindowHint(WindowHintBool.Resizable, false);

		_handle = GLFW.CreateWindow(_size.X, _size.Y, _title, null, null);
		GLFW.MakeContextCurrent(_handle);
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
}
