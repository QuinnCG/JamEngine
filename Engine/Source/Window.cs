using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GLFWWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace Engine;

public static unsafe class Window
{
	internal static GLFWWindow* Handle { get; private set; }
	internal static bool IsClosing => GLFW.WindowShouldClose(Handle);

	internal static void Launch()
	{
		GLFW.Init();

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

	public static void Close()
	{
		GLFW.SetWindowShouldClose(Handle, true);
	}
}
