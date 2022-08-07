using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ImageResizer3000.Helpers
{
	internal class ConsoleHelper
	{
		private const string blockChar = "█";
		private static void PrintProgBar(int currentFiles, int totalFiles)
		{
			Console.SetCursorPosition(0, 1);
			ClearCurrentConsoleLine();
			for (int i = 0; i < currentFiles; i++)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.CursorLeft = i;
				Console.Write(blockChar);
				Console.ResetColor();
				Console.Write($" {i + 1}/{totalFiles}");
			}
		}
		public static void PrintResizeStatus(Stopwatch totalStopwatch, int totalFiles, int currentFiles)
		{
			Console.Write($"Processed: {currentFiles}/{totalFiles}\t" +
			              $"Total time: {totalStopwatch.ElapsedMilliseconds / 1000}s\t" +
			              $"Average time: {totalStopwatch.ElapsedMilliseconds / currentFiles}ms\n");
			PrintProgBar(currentFiles, totalFiles);
		}
		public static void ClearCurrentConsoleLine()
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}
	}
}
