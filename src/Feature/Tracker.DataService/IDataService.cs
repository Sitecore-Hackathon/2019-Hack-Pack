using System;
using System.Collections;
using System.Collections.Generic;

namespace Tracker.DataService
{
    public interface IDataService
    {
        void LogEvent(string referenceId, string userName);
	    List<DbEventObject> GetAllEvents();
	    List<DbEventObject> GetLastEvents(DateTime timestamp);
    }
}
