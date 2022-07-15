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
		public int width;

		public Arguments(string[] argsFromMain)
		{
			if (argsFromMain.Length < 2)
			{
				throw new Exception("Invalid args! Please enter a valid path and a command");
			}

			DirPath = argsFromMain[0];
			Command = argsFromMain[1];
			if (argsFromMain.Length > 2)
				Width = int.Parse(argsFromMain[2].Substring(argsFromMain[2].IndexOf('=') + 1));
		}

		public string DirPath
		{
			get { return dirPath; }
			private set
			{
				if (!DirExists(value))
					throw new Exception("Error");
				dirPath = value;
			}
		}

		public string Command
		{
			get { return command; }
			private set { command = value; }
		}

		public int Width
		{
			get { return width; }
			private set
			{
				if (value < 100 || value > 1200)
					throw new Exception("Image width out of range");
				width = value;
			}
		}

		public static bool DirExists(string dirPath)
		{
			if (!Directory.Exists(dirPath))
				return false;
			return true;
		}
	}
}