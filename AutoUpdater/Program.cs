using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppSettings = AutoUpdater.Properties.Settings;

namespace AutoUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Updater AppUpdater = new Updater();

				var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
				Uri VerUri = new Uri(AppSettings.Default.KCVUpdateUrl);
				Uri FileUri = new Uri(AppSettings.Default.NowVersion);

				Console.Title = "제독업무도 바빠! 자동 업데이트 프로그램";

				Assembly asm = Assembly.LoadFrom("KanColleViewer.exe");

				AssemblyName NowAppName = asm.GetName();
				Version NowVersion = NowAppName.Version;

				AppUpdater.LoadVersion(VerUri.AbsoluteUri);
				var Checkbool = AppUpdater.IsOnlineVersionGreater(NowVersion.ToString());
				var OnlineVersion = AppUpdater.GetOnlineVersion(false);


				Console.WriteLine("어플리케이션 경로: ");
				Console.WriteLine(MainFolder.ToString());
				Console.WriteLine();

				Console.WriteLine("업데이트 버전파일 경로: ");
				Console.WriteLine(VerUri.ToString());
				Console.WriteLine();

				Console.WriteLine("현재 칸코레뷰어 버전: " + NowVersion.ToString());
				Console.WriteLine();
				Console.WriteLine("최신 칸코레뷰어 버전: " + OnlineVersion.ToString());
				Console.WriteLine();
				Console.WriteLine("온라인버전이 더 높은가?: " + Checkbool.ToString());
				Console.WriteLine();

				int statusint = 0;
				if (Checkbool)
					statusint = AppUpdater.UpdateFile(FileUri.ToString(), NowVersion.ToString());
				string status;
				if (statusint == 1) status = "성공";
				else if (statusint == -1) status = "실패";
				else status = "없음";
				Console.WriteLine("업데이트상태: " + status);
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
