using System;
using System.Collections.Generic;
using System.Web;
using LiteDB;

namespace Tracker.DataService.Database
{
    public class DbDataService : IDataService
    {
		private string dataFolder = HttpRuntime.AppDomainAppPath + "/App_Data";
	    private string dbName = @"Events.db";

		/// <summary>
		/// Logs an event in the LiteDB database storing the ID of the Core item of the clicked Content Editor ribbon button and the current Sitecore user's username.
		/// </summary>
		/// <param name="referenceId"></param>
		/// <param name="userName"></param>
		public void LogEvent(string referenceId, string userName)
        {
            DbEventObject dbEventObject = new DbEventObject
            {
                ReferenceId = referenceId,
                Timestamp = DateTime.Now,
                Username = userName.ToLowerInvariant()
            };

			using (var db = new LiteDatabase(string.Format("{0}\\{1}", dataFolder, dbName)))
            {
                var events = db.GetCollection<DbEventObject>("events");
                events.Insert(dbEventObject);
            }
        }

		/// <summary>
		/// Gets all the events recorded in the LiteDB database.
		/// </summary>
		/// <returns></returns>
	    public List<DbEventObject> GetAllEvents()
	    {
		    List<DbEventObject> allEvents = new List<DbEventObject>();
		    using (var db = new LiteDatabase(string.Format("{0}\\{1}", dataFolder, dbName)))
			{
			    var dbEvents = db.GetCollection<DbEventObject>("events");

			    allEvents.AddRange(dbEvents.FindAll());
		    }

		    return allEvents;
	    }

		/// <summary>
		/// Gets the last last events recorded in the LiteDB database since a specific date/time value.
		/// </summary>
		/// <param name="timestamp"></param>
		/// <returns></returns>
	    public List<DbEventObject> GetLastEvents(DateTime timestamp)
	    {
			List<DbEventObject> allEvents = new List<DbEventObject>();
		    using (var db = new LiteDatabase(string.Format("{0}\\{1}", dataFolder, dbName)))
			{
			    var dbEvents = db.GetCollection<DbEventObject>("events").Find(o => o.Timestamp > timestamp);

			    allEvents.AddRange(dbEvents);
		    }
		    return allEvents;
		}
    }
}
