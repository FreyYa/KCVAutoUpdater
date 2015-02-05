using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace AutoUpdater
{
	public class Updater
	{
		private XDocument VersionXML;
		public bool LoadVersion(string UpdateURL)
		{
			try
			{
				VersionXML = XDocument.Load(UpdateURL);

				if (VersionXML == null)
					return false;
			}
			catch
			{
				// Couldn't download xml file?
				return false;
			}

			return true;
		}
		public string GetOnlineVersion(bool IsSelfUpdate, bool bGetURL = false)
		{
			if (VersionXML == null)
				return "";

			IEnumerable<XElement> Versions = VersionXML.Root.Descendants("Item");

			string ElementName = !bGetURL ? "Version" : "URL";
			if (!IsSelfUpdate)
			{
				var AppVersion = Versions.Where(x => x.Element("Name").Value.Equals("App")).FirstOrDefault().Element(ElementName).Value;
				return AppVersion;
			}
			else
			{
				var AppVersion = Versions.Where(x => x.Element("Name").Value.Equals("Updater")).FirstOrDefault().Element(ElementName).Value;
				return AppVersion;
			}
		}
		public bool IsOnlineVersionGreater(bool IsSelfUpdate, string LocalVersionString)
		{
			if (VersionXML == null)
				return true;

			IEnumerable<XElement> Versions = VersionXML.Root.Descendants("Item");
			string ElementName = "Version";
			if (LocalVersionString == "알 수 없음") return false;
			else if (LocalVersionString == "없음") return false;
			Version LocalVersion = new Version(LocalVersionString);

			if (!IsSelfUpdate)
				return LocalVersion.CompareTo(new Version(Versions.Where(x => x.Element("Name").Value.Equals("App")).FirstOrDefault().Element(ElementName).Value)) < 0;
			else
				return LocalVersion.CompareTo(new Version(Versions.Where(x => x.Element("Name").Value.Equals("Updater")).FirstOrDefault().Element(ElementName).Value)) < 0;
		}
		public int UpdateFile(bool IsSelfUpdate, string BaseTranslationURL, string NowVersion)
		{
			var MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

			using (WebClient Client = new WebClient())
			{
				int ReturnValue = 0;

				try
				{
					if (!Directory.Exists(Path.Combine(MainFolder, "UpdateBin"))) Directory.CreateDirectory(Path.Combine(MainFolder, "UpdateBin"));
					if (!Directory.Exists(Path.Combine(MainFolder, "UpdateBin", "tmp"))) Directory.CreateDirectory(Path.Combine(MainFolder, "UpdateBin", "tmp"));
					if (IsSelfUpdate && !Directory.Exists(Path.Combine(MainFolder, "tmp"))) Directory.CreateDirectory(Path.Combine(MainFolder, "tmp"));
					if (IsSelfUpdate)
					{
						try
						{
							Client.DownloadFile(BaseTranslationURL, Path.Combine(MainFolder, "tmp", "updater.zip"));
							ReturnValue = 1;
						}
						catch
						{
							ReturnValue = -1;
						}

					}
					else if (NowVersion == "Forced Upgrades")
					{
						Client.DownloadFile(BaseTranslationURL, Path.Combine(MainFolder, "UpdateBin", "tmp", "kcv.zip"));

						try
						{
							if (File.Exists(Path.Combine(MainFolder, "UpdateBin", "kcv.zip")))
								File.Delete(Path.Combine(MainFolder, "UpdateBin", "kcv.zip"));
							File.Move(Path.Combine(MainFolder, "UpdateBin", "tmp", "kcv.zip"), Path.Combine(MainFolder, "UpdateBin", "kcv.zip"));
							ReturnValue = 1;

						}
						catch
						{
							ReturnValue = -1;
						}
					}
					else if (IsOnlineVersionGreater(IsSelfUpdate, NowVersion))
					{
						Client.DownloadFile(BaseTranslationURL, Path.Combine(MainFolder, "UpdateBin", "tmp", "kcv.zip"));

						try
						{
							if (File.Exists(Path.Combine(MainFolder, "UpdateBin", "kcv.zip")))
								File.Delete(Path.Combine(MainFolder, "UpdateBin", "kcv.zip"));
							File.Move(Path.Combine(MainFolder, "UpdateBin", "tmp", "kcv.zip"), Path.Combine(MainFolder, "UpdateBin", "kcv.zip"));
							ReturnValue = 1;

						}
						catch
						{
							ReturnValue = -1;
						}
					}

				}
				catch
				{
					// Failed to download files.
					return -1;
				}

				if (Directory.Exists(Path.Combine(MainFolder, "UpdateBin", "tmp"))) Directory.Delete(Path.Combine(MainFolder, "UpdateBin", "tmp"));

				return ReturnValue;
			}
		}


	}
}