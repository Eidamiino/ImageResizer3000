using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageResizer3000.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer3000
{
	internal class Program
	{
		private static string ThumbFolderName = "thumbs";

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
				imageHelper.RemoveThumbs(arguments.DirPath);
			imageHelper.DeleteAllResizedImages(arguments.DirPath);
			Console.WriteLine("Cleared!");
		}

		private static void DoThumbs(Arguments arguments)
		{
			FileHelpers.EnsureFolder($"{arguments.DirPath}{ThumbFolderName}");
			var imagePaths = FileHelpers.GetFilesOfType(arguments.DirPath, ImageHelper.AllowedExtensions);
			if (!imagePaths.Any())
			{
				Console.WriteLine("No files found");
				return;
			}

			foreach (var imagePath in imagePaths)
			{
				if (imageHelper.DeleteResizeIfExists(imagePath))
					continue;
				var stopwatch = TimerHelper.StartTimer();
				imageHelper.ResizeImageAndSave(imagePath, ImageHelper.ThumbSize,
					imagePath.Insert(imagePath.LastIndexOf('\\'), $"{ThumbFolderName}"));
				Console.WriteLine(
					$"Image thumb for {imagePath.Substring(0, imagePath.IndexOf('.'))} created in {TimerHelper.StopTimerGetElapsedTime(stopwatch)}ms");
			}
		}

		private static void DoResize(Arguments arguments)
		{
			var imagePaths = FileHelpers.GetFilesOfType(arguments.DirPath, ImageHelper.AllowedExtensions);
			if (!imagePaths.Any())
			{
				Console.WriteLine("No files found");
				return;
			}

			foreach (var imagePath in imagePaths)
			{
				if (imageHelper.DeleteResizeIfExists(imagePath))
					continue;
				var stopwatch = TimerHelper.StartTimer();
				imageHelper.ResizeImageAndSave(imagePath, arguments.Width, imagePath);
				Console.WriteLine(
					$"Image {imagePath.Substring(0, imagePath.IndexOf('.'))} resized in {TimerHelper.StopTimerGetElapsedTime(stopwatch)}ms");
			}
		}

		private static void Initialize(string thumbFolderPath)
		{
			FileHelpers.EnsureFolder(thumbFolderPath);
			imageHelper = new ImageHelper(thumbFolderPath);
		}
	}
}