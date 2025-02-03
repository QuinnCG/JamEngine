using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Engine;

public static class Log
{
	[DebuggerHidden]
	public static void Info(object message)
	{
		Write(Console.Out, message, ConsoleColor.Gray);
	}
	[DebuggerHidden]
	public static void Info(string category, object message)
	{
		Write(Console.Out, message, ConsoleColor.Gray, category);
	}

	[DebuggerHidden]
	public static void Warning(object message)
	{
		Write(Console.Out, message, ConsoleColor.Yellow);
	}
	[DebuggerHidden]
	public static void Warning(string category, object message)
	{
		Write(Console.Out, message, ConsoleColor.Yellow, category);
	}

	[DebuggerHidden, DoesNotReturn]
	public static void Error(object message)
	{
		Write(Console.Error, message, ConsoleColor.Red);
		Debug.Assert(false);
	}
	[DebuggerHidden, DoesNotReturn]
	public static void Error(string category, object message)
	{
		Write(Console.Error, message, ConsoleColor.Red, category);
		Debug.Assert(false);
	}

	[DebuggerHidden, DoesNotReturn]
	public static void Fatal(object message)
	{
		Write(Console.Error, message, ConsoleColor.DarkRed);
		Debug.Assert(false);
	}
	[DebuggerHidden, DoesNotReturn]
	public static void Fatal(string category, object message)
	{
		Write(Console.Error, message, ConsoleColor.DarkRed, category);
		Debug.Assert(false);
	}

	[DebuggerHidden]
	public static void Assert([DoesNotReturnIf(true)] bool condition)
	{
		Debug.Assert(condition);
	}
	[DebuggerHidden]
	public static void Assert([DoesNotReturnIf(true)] bool condition, object message)
	{
		Debug.Assert(condition);
		Error(message);
	}
	[DebuggerHidden]
	public static void Assert([DoesNotReturnIf(true)] bool condition, string category, object message)
	{
		Debug.Assert(condition);
		Fatal(category, message);
	}

	private static void Write(TextWriter stream, object message, ConsoleColor color, string? category = null)
	{
		var time = DateTime.Now.TimeOfDay;
		string timestamp = $"[{time.Hours}:{time.Minutes}:{time.Seconds}]";

		string cat = category == null ? string.Empty : $" [{category}]";

		Console.ForegroundColor = color;
		stream.WriteLine($"{timestamp}{cat}: {message}");
		stream.Flush();
		Console.ResetColor();
	}
}
