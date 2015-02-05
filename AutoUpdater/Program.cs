using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AppSettings = AutoUpdater.Properties.Settings;
using KCVKiller;

namespace AutoUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			KCVKillers shut = new KCVKillers();
			bool Existargs = false;
			var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
			if (Directory.Exists(Path.Combine(MainFolder, "UpdateBin")))
				Directory.Delete(Path.Combine(MainFolder, "UpdateBin"), true);
			try
			{
				if (args != null)
					if (args.Length > 0)
					{
						if (args[0] == "renew")
						{
							try
							{
								if (Directory.Exists(Path.Combine(MainFolder, "tmp")))
									Directory.Delete(Path.Combine(MainFolder, "tmp"), true);
								Existargs = true;
							}
							catch { }
						}
					}
				UpdaterCore updatercore = new UpdaterCore();
				//Options options = new Options();


				var up = updatercore.UpperFolder(MainFolder);

				if (!Existargs && File.Exists(Path.Combine(up, "AutoUpdater.exe")))
				{
					Console.WriteLine("상위폴더에 AutoUpdater.exe가 감지되었습니다. 자가업데이트를 시행합니다.");
					updatercore.Deflate.CopyFolder(MainFolder, up, true);
					shut = null;
					Process MyProcess = new Process();
					MyProcess.StartInfo.FileName = "AutoUpdater.exe";
					MyProcess.StartInfo.WorkingDirectory = up;
					MyProcess.StartInfo.Arguments = "renew";
					MyProcess.Start();
					MyProcess.Refresh();
					//Process.Start(Path.Combine(up, "AutoUpdater.exe"), "renew");

				}
				else
				{
					Console.Title = "제독업무도 바빠! 자동 업데이트 프로그램";



					shut.KCV();

					var verticalKCV = "KanColleViewer.exe";
					var horizontalKCV = "KanColleViewer-Horizontal.exe";

					if (!Existargs)
					{
						updatercore.Updater(true, MainFolder, "AutoUpdater.exe");
					}
					if (updatercore.UpdateUpdater) return;
					if (File.Exists(Path.Combine(MainFolder, verticalKCV)))
					{
						updatercore.Updater(false, MainFolder, verticalKCV);
					}
					else if (File.Exists(Path.Combine(MainFolder, horizontalKCV)))
					{
						updatercore.Updater(false, MainFolder, horizontalKCV);
					}
					else//파일이 없는경우
					{
						Console.WriteLine();
						Console.WriteLine("제독업무도 바빠!의 실행파일이 없습니다!");
						Console.WriteLine("(KanColleViewer.exe OR KanColleViewer-Horizontal.exe)");
						Console.Write("최신버전을 새로 다운로드/설치하시겠습니까?(Y/N): ");
						var t = System.Console.ReadLine();
						if (t.Length > 0)
							if (t.Length > 0 && t[0].ToString() == "y" || t[0].ToString() == "Y" || t[0].ToString() == "ㅛ") updatercore.Updater(false, MainFolder);

					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine("에러발생 : ");
				Console.WriteLine(e.Message);
			}
		}
	}
}
