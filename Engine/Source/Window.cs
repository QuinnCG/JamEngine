using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GLFWWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace Engine;

public static unsafe class Window
{
	public static Vector2i Resolution
	{
		get
		{
			GLFW.GetWindowSize(Handle, out int width, out int height);
			return new Vector2i(width, height);
		}
	}

	internal static GLFWWindow* Handle { get; private set; }
	internal static bool IsClosing => GLFW.WindowShouldClose(Handle);

	internal static void Launch()
	{
		GLFW.Init();

		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);

		// TODO: These should be settings as part of some launch config.
		GLFW.WindowHint(WindowHintInt.Samples, 2);
		GLFW.WindowHint(WindowHintBool.Resizable, false);

		Handle = GLFW.CreateWindow(1200, 800, "Jam Engine - Sandbox", null, null);
		GLFW.MakeContextCurrent(Handle);

		GL.LoadBindings(new GLFWBindingsContext());
	}

	internal static void PollEvents()
	{
		GLFW.PollEvents();
	}

	internal static void SwapBuffers()
	{
		GLFW.SwapBuffers(Handle);
	}

	internal static void Close()
	{
		GLFW.SetWindowShouldClose(Handle, true);
	}

	internal static void CleanUp()
	{
		GLFW.Terminate();
	}
}
