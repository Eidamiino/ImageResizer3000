using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageResizer3000
{
	internal class FileHelpers
	{
		public static void CreateFolderIfDoesntExist(string path)
		{
			if (!Arguments.DirExists(path))
				Directory.CreateDirectory(path);
		}

		public static List<string> GetFilesOfType(string path, List<string> extensions)
		{
			var allFilesPaths = Directory
				.GetFiles(path)
				.Where(x => extensions.Any(x.ToLower().EndsWith))
				.ToList();
			return allFilesPaths;
		}
	}
}
