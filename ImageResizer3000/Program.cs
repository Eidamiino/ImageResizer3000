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

		static void Main(string[] args)
		{
			Arguments arguments = new Arguments(args);
			//make a function for all the shit that just repeats
			switch (arguments.command.TrimStart('-'))
			{
				//add check for files already existing
				case "r":
				case "resize":
				{
					var imagesPath = GetFilesOfType(arguments.dirPath, AllowedExtensions);
					foreach (var imagePath in imagesPath)
					{
						var stopwatch = StartTimer();
						using Image image = Image.Load(imagePath);
						image.Mutate(x => x.Resize(arguments.width, 0));
						string outPath = imagePath.Insert(imagePath.IndexOf('.'), $".{arguments.width}");
						image.Save(outPath);
						Console.WriteLine(
							$"Image {imagePath.Substring(0, imagePath.IndexOf('.'))} resized in {GetElapsedTime(stopwatch)}ms");
					}
				}
					break;

				case "t":
				case "thumbs":
				{
					CreateFolderIfDoesNotExist($"{arguments.dirPath}\\thumbs");
					var imagePaths = GetFilesOfType(arguments.dirPath, AllowedExtensions);
					foreach (var imagePath in imagePaths)
					{
						if (imagePath.Substring(imagePath.IndexOf('.')).Contains($".{ThumbSize}."))
							File.Delete(imagePath);
						var stopwatch = StartTimer();
						using Image image = Image.Load(imagePath);
						image.Mutate(x => x.Resize(ThumbSize, 0));
						string outPath = imagePath.Insert(imagePath.IndexOf('.'), $".{ThumbSize}");
						outPath = outPath.Insert(outPath.LastIndexOf('\\'), "\\thumbs");
						image.Save(outPath);
						Console.WriteLine($"Image thumb for {imagePath.Substring(0, imagePath.IndexOf('.'))} created in {GetElapsedTime(stopwatch)}ms");
					}
				}
					break;
				case "c":
				case "clean":
				{
					//delete all files first
					//currently only checks for images with 75 in its name instead of all numbered files, should work with two indexof '.' but I want to sleep man
					Directory.Delete($"{arguments.dirPath}\\thumbs");
					var imagePaths = GetFilesOfType(arguments.dirPath, AllowedExtensions);
					foreach (var imagePath in imagePaths)
					{
						if (imagePath.Substring(imagePath.IndexOf('.')).Contains($".{ThumbSize}."))
							File.Delete(imagePath);
					}
				}
					break;
				default:
				{
					throw new Exception("Incorrect args!");
				}
			}
		}


		private static string ReadValue(string label, string defaultValue)
		{
			Console.Write($"{label} (default: {defaultValue}): ");
			string value = Console.ReadLine();
			if (value == "")
				return defaultValue;
			return value;
		}

		private static void CreateFolderIfDoesNotExist(string path)
		{
			if (!Arguments.DirExists(path))
				Directory.CreateDirectory(path);
		}

		private static List<string> GetFilesOfType(string path, List<string> extensions)
		{
			var allImagesPath = Directory
				.GetFiles(path)
				.Where(x => extensions.Any(x.ToLower().EndsWith))
				.ToList();
			if (!allImagesPath.Any())
				throw new Exception("Entered directory does not contain any jpg/jpeg files to resize");
			return allImagesPath;
		}

		private static Stopwatch StartTimer()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		private static int GetElapsedTime(Stopwatch stopwatch)
		{
			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			return ts.Milliseconds;
		}
	}
}