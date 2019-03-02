using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Tracker.DataService;
using Tracker.DataService.Database;

namespace Tracker.services
{
    /// <summary>
    /// HttpHandler invoked to process tracking button clicks requests
    /// </summary>
    public class EventLogger : IHttpHandler
    {
        private readonly IDataService _dataService = new DbDataService();

	    private readonly string ChunkFolderItemId = "{66738F84-31A9-43C9-9FCF-1515A849D6C5}";
	    private readonly string MenuFolderItemId = "{3BE34B62-23BF-491E-AB1E-E8D70ABB1183}";
	    private readonly string ChunkTemplateId = "{8F3D8F9B-2D76-4ACE-803F-35415D2B230A}";


		public void ProcessRequest(HttpContext context)
        {
            string jsonBody;

            using (var ms = new MemoryStream())
            {
                context.Request.InputStream.CopyTo(ms);
                jsonBody = Encoding.UTF8.GetString(ms.ToArray());
            }

            EventObject eventObject = JsonConvert.DeserializeObject<EventObject>(jsonBody);

            ProcessRequest(context, eventObject);
        }

        public void ProcessRequest(HttpContext context, EventObject eventObject)
        {
            context.Response.ContentType = "application/json";

            string foundButton = LocateButton(eventObject);

            if (!string.IsNullOrEmpty(foundButton))
            {
                _dataService.LogEvent(foundButton, Sitecore.Context.GetUserName());
                context.Response.Write("{ success = true }");
            }
            else
            {
                context.Response.Write("{ success = false }");
            }
        }

		/// <summary>
		/// Locates a button item in the Core database
		/// </summary>
		/// <param name="eventObject"></param>
		/// <returns></returns>
        private string LocateButton(EventObject eventObject)
        {
            Database coreDb = Database.GetDatabase("core");

            Item chunkFolder = coreDb.GetItem(ChunkFolderItemId);

            Item foundItem = LocateButton(eventObject, chunkFolder, "Header");

            if (foundItem != null)
            {
                return foundItem.ID.ToString();
            }

            Item menuFolder = coreDb.GetItem(MenuFolderItemId);

            foundItem = LocateButton(eventObject, menuFolder, "Display name");

            return foundItem == null ? null : foundItem.ID.ToString();
        }

		/// <summary>
		/// Locates a button item in the Core database querying by a field name
		/// </summary>
		/// <param name="eventObject"></param>
		/// <param name="child"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
        private Item LocateButton(EventObject eventObject, Item child, string fieldName)
        {
			// Always exclude Save button, since it is always rendered on the ribbon
	        if (eventObject.header.ToLower() == "save")
	        {
		        return null;
	        }

	        if (child[fieldName] == eventObject.header && child.TemplateID != new ID(ChunkTemplateId))
            {
                return child;
            }

            foreach (Item item in child.Children)
            {
                Item button = LocateButton(eventObject, item, fieldName);

                if (button != null)
                {
                    return button;
                }
            }

            return null;
        }

		public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}