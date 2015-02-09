using System.Diagnostics;

namespace KCVKiller
{
	public class KCVKillers
	{
		public string processName { get; set; }
		public void KCV()
		{
			processName = string.Empty;
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.StartsWith("KanColleViewer") || process.ProcessName.StartsWith("KanColleViewer-Horizontal"))
				{
					if (!process.ProcessName.Contains("vshost"))
					{
						processName = process.ProcessName;
						process.Kill();
					}
				}
			}
		}
	}
}
