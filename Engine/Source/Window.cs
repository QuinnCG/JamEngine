using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GLFWWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace Engine;

// TODO: Support fullscreen.
public static unsafe class Window
{
	public const string LogCategory = "Window";

	public static string Title
	{
		get => _title;
		set
		{
			_title = value;

			if (IsLaunched)
			{
				GLFW.SetWindowTitle(_handle, _title);
			}
		}
	}
	public static Vector2i Size
	{
		get => _size;
		set
		{
			_size = value;

			if (IsLaunched)
			{
				GLFW.SetWindowSize(_handle, value.X, value.Y);
			}
		}
	}
	public static bool IsResizable
	{
		get => _isResizable;
		set
		{
			if (IsLaunched)
			{
				Log.Error(LogCategory, "Can't set Window.IsResizable after the window has launched!");
				return;
			}

			_isResizable = value;
		}
	}
	/// <summary>
	/// Multisampled Antialiasing.<br/>
	/// Higher values are worse for performance but will have less jagged edges.<br/>
	/// Common values are 2, 4, and 8.
	/// </summary>
	/// <remarks>Default value is set to 2.</remarks>
	public static int MSAA
	{
		get => _msaa;
		set
		{
			if (IsLaunched)
			{
				Log.Error(LogCategory, "Can't set Window.MSAA after the window has launched!");
				return;
			}

			_msaa = value;
		}
	}

	internal static bool IsClosing
	{
		get => GLFW.WindowShouldClose(_handle);
	}

	private static string _title = "Jam Engine";
	private static Vector2i _size = new(1280, 720);
	private static bool _isResizable = true;
	private static int _msaa = 2;

	private static GLFWWindow* _handle;

	public static bool IsLaunched { get; private set; }

	internal static void Launch()
	{
		if (!IsLaunched)
		{
			IsLaunched = true;

			GLFW.Init();

			GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
			GLFW.WindowHint(WindowHintBool.Resizable, _isResizable);
			GLFW.WindowHint(WindowHintInt.Samples, 2);
			GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
			GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);

			_handle = GLFW.CreateWindow(_size.X, _size.Y, _title, null, null);
			GLFW.MakeContextCurrent(_handle);

			GL.LoadBindings(new GLFWBindingsContext());
			GLFW.SetWindowSizeCallback(_handle, OnWindowResize);
		}
	}

	internal static void PollEvents()
	{
		GLFW.PollEvents();
	}

	internal static void SwapBuffers()
	{
		GLFW.SwapBuffers(_handle);
	}

	public static void Close()
	{
		if (IsLaunched)
		{
			GLFW.SetWindowShouldClose(_handle, true);
		}

		IsLaunched = false;
	}

	internal static void CleanUp()
	{
		GLFW.DestroyWindow(_handle);
		GLFW.Terminate();
	}

	private static void OnWindowResize(GLFWWindow* window, int width, int height)
	{
		GL.Viewport(0, 0, width, height);
		_size = new(width, height);
	}
}
