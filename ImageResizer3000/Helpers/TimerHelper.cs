using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ImageResizer3000
{
	internal class Helpers
	{
		public static Stopwatch StartTimer()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		public static int StopTimerGetElapsedTime(Stopwatch stopwatch)
		{
			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			return ts.Milliseconds;
		}
	}
}
