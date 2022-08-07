using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageResizer3000.Helpers;

namespace ImageResizer3000
{
	internal class Program
	{
		private static string ThumbFolderName = "thumbs";
		private static OrderedDictionary ResizedImages = new OrderedDictionary();
		private static int MaxLinesOnScreen = 15;

		private static string thumbFolderPath;
		private static ImageHelper imageHelper;

		static void Main(string[] args)
		{
			thumbFolderPath = new DirectoryInfo($".\\{ThumbFolderName}").FullName;

			Initialize(thumbFolderPath);

			Arguments arguments;
			try
			{
				arguments = new Arguments(args);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}

			switch (arguments.Command)
			{
				case Arguments.CommandType.Resize:
					{
						DoResize(arguments);
					}
					break;

				case Arguments.CommandType.Thumbs:
				{
					DoThumbs(arguments);
				}
					break;
				case Arguments.CommandType.Clean:
				{
					DoClean(arguments);
				}
					break;
			}
		}
		private static void DoClean(Arguments arguments)
		{
			if (Directory.Exists(thumbFolderPath))
				imageHelper.RemoveThumbs(arguments.DirPath, ThumbFolderName);
			imageHelper.DeleteAllResizedImages(arguments.DirPath);
			Console.WriteLine("Cleared!");
		}

		private static void DoThumbs(Arguments arguments)
		{
			FileHelpers.EnsureFolder($"{arguments.DirPath}\\{ThumbFolderName}");
			var imagePaths = FileHelpers.GetFilesOfType(arguments.DirPath, ImageHelper.AllowedExtensions);
			if (!imagePaths.Any())
			{
				Console.WriteLine("No files found");
				return;
			}

			var i = 0;
			var totalStopwatch = new Stopwatch();
			totalStopwatch.Start();

			foreach (var imagePath in imagePaths)
			{
				i++;
				if (imageHelper.DeleteResizeIfExists(imagePath))
					continue;
				var stopwatch = TimerHelper.StartTimer();
				imageHelper.ResizeImageAndSave(imagePath, ImageHelper.ThumbSize,
					imagePath.Insert(imagePath.LastIndexOf('\\'), $"\\{ThumbFolderName}"));
				Console.SetCursorPosition(0,i+1);

				if (ResizedImages.Count == MaxLinesOnScreen)
				{
					ResizedImages.RemoveAt(0);
				}
					
				ResizedImages.Add(imagePath, TimerHelper.StopTimerGetElapsedTime(stopwatch));
				Console.SetCursorPosition(0, 2);
				foreach (DictionaryEntry dictionaryEntry in ResizedImages)
				{
					ConsoleHelper.ClearCurrentConsoleLine();
					Console.WriteLine(
						$"Image thumb for {dictionaryEntry.Key.ToString().Substring(0,dictionaryEntry.Key.ToString().IndexOf('.'))} created in {dictionaryEntry.Value}ms");
				}
				ClearAndPrint(totalStopwatch, imagePaths, i);
			}
			totalStopwatch.Stop();
			Console.SetCursorPosition(0,MaxLinesOnScreen+5);
		}

		private static void ClearAndPrint(Stopwatch totalStopwatch, List<string> imagePaths, int i)
		{
			Console.SetCursorPosition(0, 0);
			ConsoleHelper.ClearCurrentConsoleLine();
			ConsoleHelper.PrintResizeStatus(totalStopwatch, imagePaths.Count, i);
		}

		private static void DoResize(Arguments arguments)
		{
			var imagePaths = FileHelpers.GetFilesOfType(arguments.DirPath, ImageHelper.AllowedExtensions);
			if (!imagePaths.Any())
			{
				Console.WriteLine("No files found");
				return;
			}

			var i = 0;
			var totalStopwatch = new Stopwatch();
			totalStopwatch.Start();

			foreach (var imagePath in imagePaths)
			{
				i++;
				if (imageHelper.DeleteResizeIfExists(imagePath))
					continue;
				var stopwatch = TimerHelper.StartTimer();
				imageHelper.ResizeImageAndSave(imagePath, arguments.Width, imagePath);
				Console.SetCursorPosition(0, i + 1);

				if (ResizedImages.Count == MaxLinesOnScreen)
				{
					ResizedImages.RemoveAt(0);
				}

				ResizedImages.Add(imagePath, TimerHelper.StopTimerGetElapsedTime(stopwatch));
				Console.SetCursorPosition(0, 2);
				foreach (DictionaryEntry dictionaryEntry in ResizedImages)
				{
					ConsoleHelper.ClearCurrentConsoleLine();
					Console.WriteLine(
						$"Image {dictionaryEntry.Key.ToString().Substring(0, dictionaryEntry.Key.ToString().IndexOf('.'))} resized in {dictionaryEntry.Value}ms");
				}
				ClearAndPrint(totalStopwatch, imagePaths, i);
			}
			totalStopwatch.Stop();
			Console.SetCursorPosition(0, MaxLinesOnScreen+5);
		}

		private static void Initialize(string thumbFolderPath)
		{
			FileHelpers.EnsureFolder(thumbFolderPath);
			imageHelper = new ImageHelper(thumbFolderPath);
		}
	}
}