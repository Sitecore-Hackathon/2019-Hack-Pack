using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.StringExtensions;

namespace Tracker.SitecoreExtensions
{
	/// <summary>
	/// Pipeline processor to inject custom javascript and stylesheets in the "head" section of the Content Editor
	/// Credits: https://jammykam.wordpress.com/2014/04/24/adding-custom-javascript-and-stylesheets-in-the-content-editor/
	/// </summary>
	public class InjectScripts
	{
		private const string JavascriptTag = "<script src=\"{0}\"></script>";
		private const string StylesheetLinkTag = "<link href=\"{0}\" rel=\"stylesheet\" />";

		public void Process(PipelineArgs args)
		{
			AddControls(JavascriptTag, "CustomContentEditorJavascript");
			AddControls(StylesheetLinkTag, "CustomContentEditorStylesheets");
		}

		private void AddControls(string resourceTag, string configKey)
		{
			Assert.IsNotNullOrEmpty(configKey, "Content Editor resource config key cannot be null");

			string resources = Sitecore.Configuration.Settings.GetSetting(configKey);

			if (String.IsNullOrEmpty(resources))
				return;

			foreach (var resource in resources.Split('|'))
			{
				Sitecore.Context.Page.Page.Header.Controls.Add((Control)new LiteralControl(resourceTag.FormatWith(resource)));
			}
		}
	}
}
