using System;
using System.IO;
using System.Reflection;
using AppSettings = AutoUpdater.Properties.Settings;
using System.Diagnostics;

namespace AutoUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				UpdaterCore Updater = new UpdaterCore();
				Console.Title = "제독업무도 바빠! 자동 업데이트 프로그램";
				var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

				var verticalKCV = Path.Combine(MainFolder,"KanColleViewer.exe");
				var horizontalKCV = Path.Combine(MainFolder, "KanColleViewer-Horizontal.exe");


				if (File.Exists(verticalKCV))
				{
					Updater.Updater(MainFolder, verticalKCV);
				}
				else if (File.Exists(horizontalKCV))
				{
					Updater.Updater(MainFolder, horizontalKCV);
				}
				else//파일이 없는경우
				{
					Console.WriteLine("제독업무도 바빠!의 실행파일이 없습니다!");
					Console.WriteLine("(KanColleViewer.exe OR KanColleViewer-Horizontal.exe)");
					Console.Write("최신버전을 새로 다운로드/설치하시겠습니까?(Y/N): ");
					var t=System.Console.ReadLine();
					if (t[0].ToString() == "y" || t[0].ToString() == "Y" || t[0].ToString() == "ㅛ") Updater.Updater(MainFolder); ;
						
				}

			}
			catch (Exception e)
			{
				Console.WriteLine("에러발생 : ");
				Console.WriteLine(e.Message);
			}
			//System.Console.ReadKey();
		}
	}
}
