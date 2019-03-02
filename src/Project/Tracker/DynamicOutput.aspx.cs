using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Tracker.DataService;
using Tracker.ProcessingTask;

namespace Tracker
{
	/// <summary>
	/// Test page to troubleshoot/debug dynamic output generated to render buttons
	/// </summary>
	public partial class DynamicOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            rptData.DataSource = EventDataStore.GetGlobalMostFrequent(10);
            rptData.DataBind();
        }

        protected void rptData_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DbEventObjectWithCount eventObject = e.Item.DataItem as DbEventObjectWithCount;

            if (eventObject == null)
            {
                return;
            }

            Literal litTitle = (Literal)e.Item.FindControl("litTitle");

            if (litTitle != null)
            {
                Database core = Database.GetDatabase("core");

                Item item = core.GetItem(eventObject.Object.ReferenceId);

                if (item != null)
                {
                    litTitle.Text = item["Header"] + " " + eventObject.Count;
                }
                else
                {
	                litTitle.Text = eventObject.Object.ReferenceId + " " + eventObject.Count;
                }
            }
        }
    }
}