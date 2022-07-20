using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer3000.Helpers
{
	internal class ImageHelper
	{
		private readonly string thumbFolderPath;
		
		public static List<string> AllowedExtensions { get; } = new List<string> { ".jpg", ".jpeg" };
		
		public int ThumbSize { get; }

		public ImageHelper(string thumbFolderPath, int thumbSize = 75)
		{
			this.thumbFolderPath = thumbFolderPath;
			ThumbSize = thumbSize;
		}

		public void DeleteAllResizedImages(string dirPath)
		{
			var imagePaths = FileHelpers.GetFilesOfType(dirPath, AllowedExtensions);
			foreach (var imagePath in imagePaths)
			{
				if (imagePath.Substring(imagePath.IndexOf('.') + 1).Any(char.IsDigit))
					File.Delete(imagePath);
			}
		}

		public bool DeleteResizeIfExists(string imagePath)
		{
			if (imagePath.Substring(imagePath.IndexOf('.') + 1, 2).Any(char.IsDigit))
			{
				File.Delete(imagePath);
				return true;
			}

			return false;
		}

		public void ResizeImageAndSave(string path, int width, string pathToSave)
		{
			using Image image = Image.Load(path);
			image.Mutate(x => x.Resize(width, 0));
			string outPath = pathToSave.Insert(pathToSave.IndexOf('.'), $".{width}");
			image.Save(outPath);
		}

		public void RemoveThumbs(string dirPath)
		{
			var thumbPaths = Directory.GetFiles($"{dirPath}{thumbFolderPath}");
			foreach (var thumbPath in thumbPaths)
			{
				File.Delete(thumbPath);
			}

			Directory.Delete($"{dirPath}{thumbFolderPath}");
		}
	}
}
