using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer3000
{
	internal class ImageHelpers
	{
		public static List<string> AllowedExtensions = new List<string> { ".jpg", ".jpeg" };
		public const string ThumbFolderName = "\\thumbs";
		public const int ThumbSize = 75;
		public static void DeleteAllResizedImages(string dirPath)
		{
			var imagePaths = FileHelpers.GetFilesOfType(dirPath, AllowedExtensions);
			foreach (var imagePath in imagePaths)
			{
				if (imagePath.Substring(imagePath.IndexOf('.') + 1).Any(char.IsDigit))
					File.Delete(imagePath);
			}
		}

		public static bool DeleteResizeIfExists(string imagePath)
		{
			if (imagePath.Substring(imagePath.IndexOf('.') + 1, 2).Any(char.IsDigit))
			{
				File.Delete(imagePath);
				return true;
			}

			return false;
		}

		public static void ResizeImageAndSave(string path, int width, string pathToSave)
		{
			using Image image = Image.Load(path);
			image.Mutate(x => x.Resize(width, 0));
			string outPath = pathToSave.Insert(pathToSave.IndexOf('.'), $".{width}");
			image.Save(outPath);
		}

		public static void RemoveThumbs(string dirPath)
		{
			var thumbPaths = Directory.GetFiles($"{dirPath}{ThumbFolderName}");
			foreach (var thumbPath in thumbPaths)
			{
				File.Delete(thumbPath);
			}

			Directory.Delete($"{dirPath}{ThumbFolderName}");
		}
	}
}
