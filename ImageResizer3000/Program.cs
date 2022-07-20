using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer3000
{
	internal class Program
	{
		static void Main(string[] args)
		{
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
					if (arguments.Width == 0)
					{
						Console.WriteLine("Invalid width");
						break;
					}
					var imagePaths = FileHelpers.GetFilesOfType(arguments.DirPath, ImageHelpers.AllowedExtensions);
					if (!imagePaths.Any())
						break;

					foreach (var imagePath in imagePaths)
					{
						if (ImageHelpers.DeleteResizeIfExists(imagePath))
							continue;
						var stopwatch = Helpers.StartTimer();
						ImageHelpers.ResizeImageAndSave(imagePath, arguments.Width, imagePath);
						Console.WriteLine(
							$"Image {imagePath.Substring(0, imagePath.IndexOf('.'))} resized in {Helpers.StopTimerGetElapsedTime(stopwatch)}ms");
					}
				}
					break;

				case Arguments.CommandType.Thumbs:
				{
					FileHelpers.CreateFolderIfDoesntExist($"{arguments.DirPath}{ImageHelpers.ThumbFolderName}");
					var imagePaths = FileHelpers.GetFilesOfType(arguments.DirPath, ImageHelpers.AllowedExtensions);
					if (!imagePaths.Any())
						break;

					foreach (var imagePath in imagePaths)
					{
						if (ImageHelpers.DeleteResizeIfExists(imagePath))
							continue;
						var stopwatch = Helpers.StartTimer();
						ImageHelpers.ResizeImageAndSave(imagePath, ImageHelpers.ThumbSize,
							imagePath.Insert(imagePath.LastIndexOf('\\'), $"{ImageHelpers.ThumbFolderName}"));
						Console.WriteLine(
							$"Image thumb for {imagePath.Substring(0, imagePath.IndexOf('.'))} created in {Helpers.StopTimerGetElapsedTime(stopwatch)}ms");
					}
				}
					break;
				case Arguments.CommandType.Clean:
				{
					if (Directory.Exists($"{arguments.DirPath}{ImageHelpers.ThumbFolderName}"))
						ImageHelpers.RemoveThumbs(arguments.DirPath);
					ImageHelpers.DeleteAllResizedImages(arguments.DirPath);
					Console.WriteLine("Cleared!");
				}
					break;
			}
		}
	}
}