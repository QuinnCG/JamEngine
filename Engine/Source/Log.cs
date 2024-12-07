using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Engine;

public static class Log
{
	public static void Info(object message)
	{
		Print(string.Empty, message, ConsoleColor.Gray);
	}
	public static void Info(string type, object message)
	{
		Print(type, message, ConsoleColor.Gray);
	}

	public static void Warning(object message)
	{
		Print(string.Empty, message, ConsoleColor.Yellow);
	}
	public static void Warning(string type, object message)
	{
		Print(type, message, ConsoleColor.DarkYellow);
	}

	public static void Error(object message)
	{
		Print(string.Empty, message, ConsoleColor.Red);
	}
	public static void Error(string type, object message)
	{
		Print(type, message, ConsoleColor.Red);
	}

	public static void Fatal(object message)
	{
		Print(string.Empty, message, ConsoleColor.DarkRed);
	}
	public static void Fatal(string type, object message)
	{
		Print(type, message, ConsoleColor.DarkRed);
	}

	public static void Assert([DoesNotReturnIf(false)] bool condition)
	{
		Debug.Assert(condition);
	}
	public static void Assert([DoesNotReturnIf(false)] bool condition, string message)
	{
		Debug.Assert(condition, message);
	}

	[DoesNotReturn]
	public static void Break()
	{
		Debug.Assert(false);
	}

	private static void Print(string type, object message, ConsoleColor color)
	{
		var time = DateTime.Now.TimeOfDay;

		string typeSegment = string.IsNullOrWhiteSpace(type) ? string.Empty : $"[{type}]";

		Console.ForegroundColor = color;
		Console.WriteLine($"[{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}]{typeSegment}: {message}");
		Console.ResetColor();
	}
}
