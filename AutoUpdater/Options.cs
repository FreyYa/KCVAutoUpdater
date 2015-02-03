using CommandLine;
using CommandLine.Text;
using System;
using AppSettings = AutoUpdater.Properties.Settings;


namespace AutoUpdater
{
	class Options
	{
		[Option("reset_settings", Required = false,
		  HelpText = "Reset settings")]
		public bool ResetSettings { get; set; }

		public void ApplyOptions(Options options)
		{
			if (options.ResetSettings)
			{
				AppSettings.Default.Reset();
				AppSettings.Default.Reload();
			}
		}

	}

}
