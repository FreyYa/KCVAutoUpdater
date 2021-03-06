﻿using KCVKiller;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace AutoUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			ErrorReport error = new ErrorReport();

			KCVKillers shut = new KCVKillers();
			bool Existargs = false;
			var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);



			try
			{//
				if (File.Exists(Path.Combine(MainFolder, "ResetUpdaterSettings.cmd")))
					File.Delete(Path.Combine(MainFolder, "ResetUpdaterSettings.cmd"));
				if (File.Exists(Path.Combine(MainFolder, "KanColleViewer-Horizontal.exe")))
					File.Delete(Path.Combine(MainFolder, "KanColleViewer-Horizontal.exe"));
				if (File.Exists(Path.Combine(MainFolder, "KanColleViewer-Horizontal.exe.config")))
					File.Delete(Path.Combine(MainFolder, "KanColleViewer-Horizontal.exe.config"));
				if (File.Exists(Path.Combine(MainFolder, "KanColleViewer-Horizontal.VisualElementsManifest.xml")))
					File.Delete(Path.Combine(MainFolder, "KanColleViewer-Horizontal.VisualElementsManifest.xml"));
				if (Directory.Exists(Path.Combine(MainFolder, "UpdateBin")))
					Directory.Delete(Path.Combine(MainFolder, "UpdateBin"), true);
				if (args != null)
					if (args.Length > 0)
					{
						if (args[0] == "renew")
						{
							if (Directory.Exists(Path.Combine(MainFolder, "tmp")))
								Directory.Delete(Path.Combine(MainFolder, "tmp"), true);
							Existargs = true;
						}
					}
				UpdaterCore updatercore = new UpdaterCore();


				var up = updatercore.UpperFolder(MainFolder);

				if (!Existargs && File.Exists(Path.Combine(up, "AutoUpdater.exe")))
				{
					Console.WriteLine("상위폴더에 AutoUpdater.exe가 감지되었습니다. 자가업데이트를 시행합니다.");
					Deflate.Current.CopyFolder(MainFolder, up, true);
					shut = new KCVKillers();
					Process MyProcess = new Process();
					MyProcess.StartInfo.FileName = "AutoUpdater.exe";
					MyProcess.StartInfo.WorkingDirectory = up;
					MyProcess.StartInfo.Arguments = "renew";
					MyProcess.Start();
					MyProcess.Refresh();

				}
				else
				{
					Console.Title = "제독업무도 바빠! 자동 업데이트 프로그램";
					int whileCount = 0;
					while (!shut.IsKCVDead)
					{
						Thread.Sleep(2000);
						shut.KCV();
						whileCount++;
						if (whileCount > 10) 
						{ 
							Console.WriteLine("칸코레 뷰어 프로세스가 정상적으로 종료되지않았습니다.");
							Console.WriteLine("윈도우 작업 관리자를 통해 직접 칸코레 뷰어 프로세스를 종료해주시기 바랍니다.");
							whileCount = 0;
						}

					}


					if (!Existargs)
					{
						updatercore.Core(true, MainFolder, "AutoUpdater.exe");
					}
					if (updatercore.UpdateUpdater) return;
					if (File.Exists(Path.Combine(MainFolder, "KanColleViewer.exe")))
					{
						updatercore.Core(false, MainFolder, "KanColleViewer.exe");
					}
					else//파일이 없는경우
					{
						Console.WriteLine();
						Console.WriteLine("제독업무도 바빠!의 실행파일이 없습니다!");
						Console.WriteLine();
						Console.Write("최신버전을 새로 다운로드/설치하시겠습니까?(Y/N): ");
						var t = System.Console.ReadLine();
						if (t.Length > 0)
							if (t.Length > 0 && t[0].ToString() == "y" || t[0].ToString() == "Y" || t[0].ToString() == "ㅛ") updatercore.Core(false, MainFolder);

					}
				}

			}
			catch (Exception e)
			{
				error.catcherror(e, MainFolder);
				Console.WriteLine("에러발생 : ");
				Console.WriteLine(e.Message);

			}
		}
	}
}
