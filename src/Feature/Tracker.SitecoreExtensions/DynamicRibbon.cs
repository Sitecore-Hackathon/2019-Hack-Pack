using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Shell.Web.UI.WebControls;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls.Ribbons;
using Tracker.DataService;
using Tracker.ProcessingTask;

namespace Tracker.SitecoreExtensions
{
    public class DynamicRibbon : RibbonStrip
    {
		/// <summary>
		/// Renders buttons in a strip of the ribbon in Content Editor
		/// </summary>
		/// <param name="output"></param>
		/// <param name="ribbon"></param>
		/// <param name="strip"></param>
		/// <param name="context"></param>
	    public override void Render(HtmlTextWriter output, Ribbon ribbon, Item strip, CommandContext context)
        {
            DbEventObjectWithCount[] mostFrequentUserIds = EventDataStore.GetUserMostFrequent(Sitecore.Context.GetUserName(), 6);
            RenderSection(output, ribbon, mostFrequentUserIds, "Me");

	        DbEventObjectWithCount[] mostFrequentGlobalIds = EventDataStore.GetGlobalMostFrequent(12).Where(evt => !mostFrequentUserIds.Select(uid => uid.Object.ReferenceId).Contains(evt.Object.ReferenceId)).Take(6).ToArray();
	        RenderSection(output, ribbon, mostFrequentGlobalIds, "Everyone");
		}

        private void RenderSection(HtmlTextWriter output, Ribbon ribbon, DbEventObjectWithCount[] mostFrequentIds, string title)
        {
            List<Ribbon.Chunk> chunks = new List<Ribbon.Chunk>();
            Ribbon.Chunk chunk = new Ribbon.Chunk();
            chunk.Header = Translate.Text(title);
            chunks.Add(chunk);
            foreach (DbEventObjectWithCount str in mostFrequentIds)
            {
                chunk.Buttons.Add(new Ribbon.Button()
                {
                    ReferenceID = ID.Parse(str.Object.ReferenceId)
                });
            }
            ribbon.RenderStrip(output, chunks);
        }
    }
}
