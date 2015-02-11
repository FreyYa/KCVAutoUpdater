using System;
using System.Diagnostics;
using System.Timers;

namespace KCVKiller
{
	/// <summary>
	/// http://www.csharpstudy.com/Threads/timer.aspx
	/// </summary>
	public class KCVKillers
	{
		public string processName { get; set; }
		private bool IsKCVDead { get; set; }
		private Timer timer = new Timer();
		public void KCV()
		{
			processName = string.Empty;

			
			timer.Interval = 2000;//E초
			timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			timer.Start();
		}
		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("제독업무도 바빠!의 종료를 확인중입니다...");
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.StartsWith("KanColleViewer") || process.ProcessName.StartsWith("KanColleViewer-Horizontal"))
				{
					if (!process.ProcessName.Contains("vshost"))
					{
						processName = process.ProcessName;
					}
				}
				else
				{
					IsKCVDead = true;
				}
				if (IsKCVDead)
				{
					timer.Stop();
					timer.Dispose();
					timer.Close();
				}
			}
			
		}
	}
}
