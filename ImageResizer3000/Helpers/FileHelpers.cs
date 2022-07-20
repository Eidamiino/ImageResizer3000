using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageResizer3000.Helpers
{
	internal class FileHelpers
	{
		public static void EnsureFolder(string path)
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
