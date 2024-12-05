using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Engine;

public static class Log
{
	public static void Info(string message)
	{
		Print(message, ConsoleColor.Gray);
	}

	public static void Warning(string message)
	{
		Print(message, ConsoleColor.Yellow);
	}

	public static void Error(string message)
	{
		Print(message, ConsoleColor.Red);
	}

	public static void Fatal(string message)
	{
		Print(message, ConsoleColor.DarkRed);
	}

	public static void Assert([DoesNotReturnIf(false)] bool condition)
	{
		Debug.Assert(condition);
	}
	public static void Assert([DoesNotReturnIf(false)] bool condition, string message)
	{
		Debug.Assert(condition, message);
	}

	private static void Print(string message, ConsoleColor color)
	{
		var time = DateTime.Now.TimeOfDay;

		Console.ForegroundColor = color;
		Console.WriteLine($"[{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}]: {message}");
		Console.ResetColor();
	}
}
