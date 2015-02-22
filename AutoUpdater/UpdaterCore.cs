using System;
using System.Diagnostics;
using System.IO;
using AppSettings = AutoUpdater.Properties.Settings;

namespace AutoUpdater
{
	public class UpdaterCore
	{
		public bool UpdateUpdater;
		Updater AppUpdater = new Updater();
		public Deflate Deflate = new Deflate();
		public void Updater(bool IsSelfUpdate, string MainFolder, string _str_File = "NoFile")
		{
			Uri VerUri = new Uri(AppSettings.Default.KCVUpdateUrl);
			AppUpdater.LoadVersion(VerUri.AbsoluteUri);
			Uri FileUri = new Uri(AppUpdater.GetOnlineVersion(IsSelfUpdate, true));

			if (IsSelfUpdate)
			{
				FileVersionInfo NowVersion = FileVersionInfo.GetVersionInfo(Path.Combine(MainFolder, _str_File));
				AppUpdater.LoadVersion(VerUri.AbsoluteUri);
				var Checkbool = AppUpdater.IsOnlineVersionGreater(IsSelfUpdate, NowVersion.FileVersion);
				var OnlineVersion = AppUpdater.GetOnlineVersion(IsSelfUpdate, false);

				Console.WriteLine("현재 AutoUpdater 버전: " + NowVersion.FileVersion);
				Console.WriteLine();
				Console.WriteLine("최신 AutoUpdater 버전: " + OnlineVersion.ToString());
				Console.WriteLine();
				int statusint = 0;
				if (Checkbool)
					statusint = AppUpdater.UpdateFile(IsSelfUpdate, FileUri.ToString(), NowVersion.FileVersion);
				string status;
				if (statusint == 1) status = "성공";
				else if (statusint == -1) status = "실패";
				else status = "없음";
				Console.WriteLine("업데이트파일 다운로드: " + status);
				Console.WriteLine("");
				try
				{
					if (File.Exists(Path.Combine(MainFolder, "tmp", "updater.zip")))
					{
						Deflate.ExtractZip(Path.Combine(MainFolder, "tmp", "updater.zip"), Path.Combine(MainFolder, "tmp"));
						Console.WriteLine("압축해제 완료");
						Console.WriteLine("");
						if (File.Exists(Path.Combine(MainFolder, "tmp", "updater.zip")))
							File.Delete(Path.Combine(MainFolder, "tmp", "updater.zip"));

						Process MyProcess = new Process();

						this.UpdateUpdater = true;
						MyProcess.StartInfo.FileName = "AutoUpdater.exe";
						MyProcess.StartInfo.WorkingDirectory = Path.Combine(MainFolder, "tmp");
						MyProcess.Start();
						MyProcess.Refresh();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("에러발생 : ");
					Console.WriteLine(e.Message);
				}
			}

			#region 칸코레 뷰어가 없는경우
			else if (_str_File == "NoFile")
			{
				bool Checkbool = true;


				int statusint = 0;
				if (Checkbool)
					statusint = AppUpdater.UpdateFile(IsSelfUpdate, FileUri.ToString(), "Forced Upgrades");
				string status;
				if (statusint == 1) status = "성공";
				else if (statusint == -1) status = "실패";
				else status = "없음";
				Console.WriteLine("업데이트파일 다운로드: " + status);
				Console.WriteLine("");
				try
				{
					if (File.Exists(Path.Combine(MainFolder, "UpdateBin", "kcv.zip")))
					{
						Deflate.ExtractZip(Path.Combine(MainFolder, "UpdateBin", "kcv.zip"), Path.Combine(MainFolder, "UpdateBin"));
						Console.WriteLine("압축해제 완료");
						Console.WriteLine("");
					}
					else
					{
						Console.WriteLine("업데이트를 종료합니다.");
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("에러발생 : ");
					Console.WriteLine(e.Message);
				}
				if (File.Exists(Path.Combine(MainFolder, "UpdateBin", "kcv.zip")))
					File.Delete(Path.Combine(MainFolder, "UpdateBin", "kcv.zip"));
				try
				{
					if (Directory.Exists(Path.Combine(MainFolder, "UpdateBin")))
					{

						Deflate.CopyFolder(Path.Combine(MainFolder, "UpdateBin"), MainFolder);
						Console.WriteLine("붙여넣기 완료");
						Console.WriteLine();

						string Applocate = "KanColleViewer.exe";

						Console.WriteLine();

						Process MyProcess = new Process();
						MyProcess.StartInfo.FileName = "Updatelog.txt";
						MyProcess.StartInfo.WorkingDirectory = MainFolder;
						if (File.Exists(Path.Combine(MainFolder, "Updatelog.txt"))) MyProcess.Start();
						MyProcess.StartInfo.FileName = Applocate;
						MyProcess.Start();
						MyProcess.Refresh();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("에러발생 : ");
					Console.WriteLine(e.Message);
				}
			}
			#endregion

			#region 칸코레 뷰어가 있는경우
			else if (Path.Combine(MainFolder, _str_File) != "NoFile")
			{
				bool IsBattlePluginExist;
				if (File.Exists(Path.Combine(MainFolder, "Plugins", "BattlePreview.dll")))
					IsBattlePluginExist = true;
				else IsBattlePluginExist = false;

				FileVersionInfo NowVersion = FileVersionInfo.GetVersionInfo(Path.Combine(MainFolder, _str_File));
				AppUpdater.LoadVersion(VerUri.AbsoluteUri);
				var Checkbool = AppUpdater.IsOnlineVersionGreater(IsSelfUpdate, NowVersion.FileVersion);
				var OnlineVersion = AppUpdater.GetOnlineVersion(IsSelfUpdate, false);


				Console.WriteLine("현재 제독업무도 바빠! 버전: " + NowVersion.FileVersion);
				Console.WriteLine();
				Console.WriteLine("최신 제독업무도 바빠! 버전: " + OnlineVersion.ToString());
				Console.WriteLine();

				int statusint = 0;
				if (Checkbool)
					statusint = AppUpdater.UpdateFile(IsSelfUpdate, FileUri.ToString(), NowVersion.FileVersion);
				string status;
				if (statusint == 1) status = "성공";
				else if (statusint == -1) status = "실패";
				else status = "없음";
				Console.WriteLine("업데이트파일 다운로드: " + status);
				Console.WriteLine("");
				try
				{
					if (File.Exists(Path.Combine(MainFolder, "UpdateBin", "kcv.zip")))
					{
						Deflate.ExtractZip(Path.Combine(MainFolder, "UpdateBin", "kcv.zip"), Path.Combine(MainFolder, "UpdateBin"));
						Console.WriteLine("압축해제 완료");
						Console.WriteLine("");

						if (File.Exists(Path.Combine(MainFolder, "UpdateBin", "kcv.zip")))
							File.Delete(Path.Combine(MainFolder, "UpdateBin", "kcv.zip"));
						try
						{
							if (Directory.Exists(Path.Combine(MainFolder, "UpdateBin")))
							{
								Deflate.CopyFolder(Path.Combine(MainFolder, "UpdateBin"), MainFolder);
								Console.WriteLine("붙여넣기 완료");
								Console.WriteLine("");
								if (!IsBattlePluginExist)
								{
									Console.WriteLine("기존 상태에 따라 전투 미리보기 플러그인 파일을 제거합니다.");
									if (File.Exists(Path.Combine(MainFolder, "Plugins", "BattlePreview.dll")))
										File.Delete(Path.Combine(MainFolder, "Plugins", "BattlePreview.dll"));
									Console.WriteLine("");
								}
								Console.WriteLine("업데이트를 종료합니다.");
								if (File.Exists(Path.Combine(MainFolder, _str_File)))
								{
									Process MyProcess = new Process();
									MyProcess.StartInfo.FileName = "Updatelog.txt";
									MyProcess.StartInfo.WorkingDirectory = MainFolder;
									if (File.Exists(Path.Combine(MainFolder, "Updatelog.txt"))) MyProcess.Start();
									MyProcess.StartInfo.FileName = _str_File;
									MyProcess.Start();
									MyProcess.Refresh();
								}

							}
						}
						catch (Exception e)
						{
							Console.WriteLine("에러발생 : ");
							Console.WriteLine(e.Message);
						}
					}
					else
					{
						Console.WriteLine("업데이트를 종료합니다.");
						if (File.Exists(Path.Combine(MainFolder, _str_File)))
						{
							Process MyProcess = new Process();
							MyProcess.StartInfo.FileName = "Updatelog.txt";
							MyProcess.StartInfo.WorkingDirectory = MainFolder;
							if (File.Exists(Path.Combine(MainFolder, "Updatelog.txt"))) MyProcess.Start();
							MyProcess.StartInfo.FileName = _str_File;
							MyProcess.Start();
							MyProcess.Refresh();
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("에러발생 : ");
					Console.WriteLine(e.Message);
				}
			}
			#endregion

			else Console.WriteLine("업데이트 에러!");
			//Console.WriteLine("아무키나 눌러서 업데이터를 종료해주세요");
		}
		public string UpperFolder(string MainFolder)
		{
			string[] temp = MainFolder.Split('\\');
			int i = temp.Length;

			string str = string.Empty;
			for (int j = 0; j < temp.Length - 1; j++)
			{
				if (j == 0) str = temp[j] + "\\";
				else
					str = Path.Combine(str, temp[j]);
			}

			return str;
		}
	}
}
