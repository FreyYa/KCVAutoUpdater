using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					processName = process.ProcessName;
					process.Kill();
				}
			}
		}
	}
}
