using System;
using System.Diagnostics;

namespace KCVKiller
{
	public class KCVKillers
	{
		public string processName { get; set; }
		public bool IsKCVDead { get; set; }
		private bool test { get; set; }
		public void KCV()
		{
			test = false;
			Console.WriteLine("제독업무도 바빠!의 종료를 확인중입니다...");
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.StartsWith("KanColleViewer"))
				{
					if (!process.ProcessName.Contains("vshost"))
					{
						processName = process.ProcessName;
						test = true;
					}
				}
			}
			if (test) IsKCVDead = false;
			else IsKCVDead = true;
		}
	}
}
