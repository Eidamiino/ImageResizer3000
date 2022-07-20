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
		public static List<string> AllowedExtensions = new List<string> {".jpg", ".jpeg"};
		public const int ThumbSize = 75;
		public const string ThumbFolderName = "\\thumbs";

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

			switch (arguments.Command.TrimStart('-'))
			{
				case "r":
				case "resize":
				{
					if (arguments.Width == 0)
					{
						Console.WriteLine("Invalid width");
						break;
					}

					var imagesPath = GetFilesOfType(arguments.DirPath, AllowedExtensions);
					if (!imagesPath.Any())
					{
						Console.WriteLine("No files of set type found");
						break;
					}

					foreach (var imagePath in imagesPath)
					{
						if (imagePath.Substring(imagePath.IndexOf('.') + 1).Any(char.IsDigit))
						{
							File.Delete(imagePath);
							continue;
						}

						var stopwatch = StartTimer();
						using Image image = Image.Load(imagePath);
						image.Mutate(x => x.Resize(arguments.Width, 0));
						string outPath = imagePath.Insert(imagePath.IndexOf('.'), $".{arguments.Width}");
						image.Save(outPath);
						Console.WriteLine(
							$"Image {imagePath.Substring(0, imagePath.IndexOf('.'))} resized in {StopTimerGetElapsedTime(stopwatch)}ms");
					}
				}
					break;

				case "t":
				case "thumbs":
				{
					CreateFolderIfDoesntExist($"{arguments.DirPath}{ThumbFolderName}");
					var imagePaths = GetFilesOfType(arguments.DirPath, AllowedExtensions);
					if (!imagePaths.Any())
					{
						Console.WriteLine("No files of set type found");
						break;
					}

					foreach (var imagePath in imagePaths)
					{
						if (imagePath.Substring(imagePath.IndexOf('.')).Contains($".{ThumbSize}."))
							File.Delete(imagePath);
						var stopwatch = StartTimer();
						using Image image = Image.Load(imagePath);
						image.Mutate(x => x.Resize(ThumbSize, 0));
						string outPath = imagePath.Insert(imagePath.IndexOf('.'), $".{ThumbSize}");
						outPath = outPath.Insert(outPath.LastIndexOf('\\'), $"{ThumbFolderName}");
						image.Save(outPath);
						Console.WriteLine(
							$"Image thumb for {imagePath.Substring(0, imagePath.IndexOf('.'))} created in {StopTimerGetElapsedTime(stopwatch)}ms");
					}
				}
					break;
				case "c":
				case "clean":
				{
					if (Directory.Exists($"{arguments.DirPath}{ThumbFolderName}"))
						RemoveThumbs(arguments);
					var imagePaths = GetFilesOfType(arguments.DirPath, AllowedExtensions);
					foreach (var imagePath in imagePaths)
					{
						if (imagePath.Substring(imagePath.IndexOf('.') + 1).Any(char.IsDigit))
							File.Delete(imagePath);
					}
					Console.WriteLine("Cleared!");
				}
					break;
				default:
				{
					Console.WriteLine("Incorrect args!");
					break;
				}
			}
		}

		private static void RemoveThumbs(Arguments arguments)
		{
			var thumbPaths = Directory.GetFiles($"{arguments.DirPath}{ThumbFolderName}");
			foreach (var thumbPath in thumbPaths)
			{
				File.Delete(thumbPath);
			}

			Directory.Delete($"{arguments.DirPath}{ThumbFolderName}");
		}

		private static void CreateFolderIfDoesntExist(string path)
		{
			if (!Arguments.DirExists(path))
				Directory.CreateDirectory(path);
		}

		private static List<string> GetFilesOfType(string path, List<string> extensions)
		{
			var allFilesPaths = Directory
				.GetFiles(path)
				.Where(x => extensions.Any(x.ToLower().EndsWith))
				.ToList();
			return allFilesPaths;
		}

		private static Stopwatch StartTimer()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		private static int StopTimerGetElapsedTime(Stopwatch stopwatch)
		{
			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			return ts.Milliseconds;
		}
	}
}