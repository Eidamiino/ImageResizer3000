using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageResizer3000
{
	internal class Arguments
	{
		private string dirPath;

		public enum CommandType
		{
			Resize,
			Thumbs,
			Clean
		}

		private CommandType command;
		private int width;

		public Arguments(string[] argsFromMain)
		{
			if (argsFromMain.Length < 2)
				throw new Exception("Invalid args! Please enter a valid path and a command");

			DirPath = argsFromMain[0];
			if (argsFromMain.Length > 2)
				Width = int.Parse(argsFromMain[2].Substring(argsFromMain[2].IndexOf('=') + 1));
			switch (argsFromMain[1].TrimStart('-'))
			{
				case "r":
				case "resize":
				{
					if (Width == 0)
						throw new Exception("Invalid width");
					Command = CommandType.Resize;
				}
					break;
				case "t":
				case "thumbs":
				{
					Command = CommandType.Thumbs;
				}
					break;
				case "c":
				case "clean":
				{
					Command = CommandType.Clean;
				}
					break;
				default:
				{
					throw new Exception("Incorrect args");
				}
			}
		}

		public string DirPath
		{
			get => dirPath;
			private set
			{
				if (!DirExists(value))
					throw new Exception("Directory doesn't exist");
				dirPath = value;
			}
		}

		public CommandType Command
		{
			get => command;
			private set => command = value;
		}

		public int Width
		{
			get => width;
			private set
			{
				if (value < 100 || value > 1200)
					throw new Exception("Image width out of range");
				width = value;
			}
		}

		public static bool DirExists(string dirPath)
		{
			return Directory.Exists(dirPath);
		}
	}
}