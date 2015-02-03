using System;
using System.Diagnostics;
using System.IO;
using AppSettings = AutoUpdater.Properties.Settings;

namespace AutoUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				
				UpdaterCore updater = new UpdaterCore();
				Options options = new Options();

				if (CommandLine.Parser.Default.ParseArguments(args, options))
				{
					options.ApplyOptions(options);
				}

				Console.Title = "제독업무도 바빠! 자동 업데이트 프로그램";
				var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

				foreach (Process process in Process.GetProcesses())
				{
					if (process.ProcessName.StartsWith("KanColleViewer") || process.ProcessName.StartsWith("KanColleViewer-Horizontal"))
					{
						process.Kill();
					}
				}
				if (AppSettings.Default.IsFirstUpdate)
				{
					Console.WriteLine("주로 사용하는 제독업무도 바빠!의 버전을 설정해주시기 바랍니다.");
					Console.WriteLine("해당 질문은 최초 업데이트때만 나타납니다. 설정을 나중에 바꾸려는 경우에는");
					Console.WriteLine("ResetUpdaterSettings.cmd파일을 실행하시기 바랍니다.");
					Console.WriteLine();
					Console.WriteLine("기본실행을 가로모드로 설정하는 경우 H(h)를 입력하고 Enter를 눌러주세요.");
					Console.Write("세로의 경우는 Enter만 눌러주세요");
					var t = System.Console.ReadLine();
					if (t.Length > 0)
						if (t[0].ToString() == "H" || t[0].ToString() == "h" || t[0].ToString() == "ㅗ")
							AppSettings.Default.IsHorizontal = true;
					AppSettings.Default.IsFirstUpdate = false;
					AppSettings.Default.Save();
				}

				var verticalKCV = "KanColleViewer.exe";
				var horizontalKCV = "KanColleViewer-Horizontal.exe";

				if (File.Exists(Path.Combine(MainFolder,verticalKCV)))
				{
					updater.Updater(MainFolder, verticalKCV);
					return;
				}
				else if (File.Exists(Path.Combine(MainFolder,horizontalKCV)))
				{
					updater.Updater(MainFolder, horizontalKCV);
					return;
				}
				else//파일이 없는경우
				{
					Console.WriteLine();
					Console.WriteLine("제독업무도 바빠!의 실행파일이 없습니다!");
					Console.WriteLine("(KanColleViewer.exe OR KanColleViewer-Horizontal.exe)");
					Console.Write("최신버전을 새로 다운로드/설치하시겠습니까?(Y/N): ");
					var t = System.Console.ReadLine();
					if (t.Length > 0)
						if (t.Length > 0 && t[0].ToString() == "y" || t[0].ToString() == "Y" || t[0].ToString() == "ㅛ") updater.Updater(MainFolder);
					return;

				}

			}
			catch (Exception e)
			{
				Console.WriteLine("에러발생 : ");
				Console.WriteLine(e.Message);
			}
			System.Console.ReadKey();
		}
	}
}
