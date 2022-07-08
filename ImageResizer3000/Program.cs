using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer3000
{
	internal class Program
	{
		//konstantni list hledanych pripon
		public const string SearchedExtensions = "*.jpg";

		static void Main(string[] args)

		{
			Arguments arguments = new Arguments(args);
			//var path = args[0];
			//var command = args[1];
			switch (arguments.command)
			{
				case "-r":
				case "--resize":
				{
					var allImagesPath = Directory.GetFiles(arguments.dirPath, SearchedExtensions);
					if (allImagesPath.Length == 0)
					{
						Console.WriteLine("Entered directory does not contain any jpg/jpeg files to resize");
						return;
					}

					foreach (var imagePath in allImagesPath)
					{
						string width = args[2].Substring(3);

						using Image image = Image.Load(imagePath);
						image.Mutate(x => x.Resize(0,int.Parse(width)));
						string outPath = imagePath.Insert(imagePath.IndexOf('.'), $".{width}");
						image.Save(outPath);
						Console.WriteLine($"Image {imagePath.Substring(0,imagePath.IndexOf('.'))} resized in ms");


					}
				}
					break;
			}

			{
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

		
	}
}