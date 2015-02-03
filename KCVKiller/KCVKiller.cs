using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVKiller
{
	public class KCVKiller
	{
		public void KCV(){
			var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.StartsWith("KanColleViewer") || process.ProcessName.StartsWith("KanColleViewer-Horizontal"))
				{
					process.Kill();
				}
			}
		}
	}
}
