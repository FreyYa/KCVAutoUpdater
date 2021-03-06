﻿using System.IO;
using System.IO.Compression;

namespace AutoUpdater
{
	public class Deflate
	{
		#region singleton

		private static Deflate current = new Deflate();

		public static Deflate Current
		{
			get { return current; }
		}

		#endregion


		public void ExtractZip(string fileLocation, string ExtractLocation)
		{
			ZipFile.ExtractToDirectory(fileLocation, ExtractLocation);
		}
		/// <summary>
		/// 폴더 복사 작업이 끝나면 sourceFolder를 삭제한다.
		/// </summary>
		/// <param name="sourceFolder">복사할 폴더가 있는 경로</param>
		/// <param name="destFolder">붙여넣기할 경로</param>
		public void CopyFolder(string sourceFolder, string destFolder, bool IsSelfUpdate = false)
		{
			if (!Directory.Exists(destFolder))
				Directory.CreateDirectory(destFolder);

			string[] files = Directory.GetFiles(sourceFolder);
			string[] folders = Directory.GetDirectories(sourceFolder);

			foreach (string file in files)
			{
				string name = Path.GetFileName(file);
				string dest = Path.Combine(destFolder, name);
				if (!IsSelfUpdate)
				{
					if (!file.Contains("AutoUpdater.exe"))
						if (!file.Contains("CommandLine.dll"))
							if (!file.Contains("KCVKiller.dll"))
								File.Copy(file, dest, true);
				}
				else
				{
					File.Copy(file, dest, true);
				}

			}

			// foreach 안에서 재귀 함수를 통해서 폴더 복사 및 파일 복사 진행 완료
			foreach (string folder in folders)
			{
				string name = Path.GetFileName(folder);
				string dest = Path.Combine(destFolder, name);
				CopyFolder(folder, dest, IsSelfUpdate);
			}
			if (!IsSelfUpdate) Directory.Delete(sourceFolder, true);
		}

	}
}
