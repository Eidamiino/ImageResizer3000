using System;
using System.Diagnostics;

namespace ImageResizer3000.Helpers
{
	internal class TimerHelper
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
