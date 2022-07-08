using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageResizer3000
{
	internal class Arguments
	{
		public string dirPath;
		public string command;
		public string width;

		public Arguments(string[] argsFromMain)
		{
			if (argsFromMain.Length < 2)
			{
				Console.WriteLine("Invalid args!");
				return;
			}
			DirPath = argsFromMain[0];
			command = argsFromMain[1];
			width = argsFromMain[2];
		}
		public string DirPath
		{
			get { return dirPath; }
			private set
			{
				if (!DirExists(dirPath))
					throw new Exception("Error");
				dirPath = value;
			}
		}
		public static bool DirExists(string dirPath)
		{
			if (!Directory.Exists(dirPath))
			{
				Console.WriteLine($"Directory with path {dirPath} does not exist");
				return false;
			}

			return true;
		}
	}
}