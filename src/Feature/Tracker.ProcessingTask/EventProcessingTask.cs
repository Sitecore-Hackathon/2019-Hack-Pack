
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tracker.DataService;
using Tracker.DataService.Database;

namespace Tracker.ProcessingTask
{
    public class EventStore
    {
        private readonly Dictionary<string, int> EventCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, DbEventObject> EventObjects = new Dictionary<string, DbEventObject>();

        private readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        public void Increment(DbEventObject eventObject)
        {
            string cacheKey = GetCacheKey(eventObject);

            try
            {
                Lock.EnterWriteLock();

                if (!EventCounts.ContainsKey(cacheKey))
                {
                    EventCounts.Add(cacheKey, 0);
                    EventObjects.Add(cacheKey, eventObject);
                }

                ++EventCounts[cacheKey];
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public DbEventObjectWithCount[] GetMostFrequent(int howMany)
        {
            DbEventObjectWithCount[] eventObjects;

            try
            {
                Lock.EnterReadLock();

                var frequentKeys = EventCounts.OrderByDescending(x => x.Value).Take(howMany).ToArray();

                eventObjects = frequentKeys.Where(x => EventObjects.ContainsKey(x.Key)).Select(x => new DbEventObjectWithCount() { Count = x.Value, Object = EventObjects[x.Key] }).ToArray();
            }
            finally
            {
                Lock.ExitReadLock();
            }

            return eventObjects;
        }

        private string GetCacheKey(DbEventObject eventObject)
        {
            return eventObject.ReferenceId;
        }
    }

    public static class EventDataStore
    {
        private static readonly EventStore GlobalStore = new EventStore();
        private static readonly Dictionary<string, EventStore> UserStores = new Dictionary<string, EventStore>();

        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        public static EventStore GetUserStore(string username)
        {
            EventStore store;

            username = username.ToLowerInvariant();

            try
            {
                Lock.EnterUpgradeableReadLock();

                if (UserStores.ContainsKey(username))
                {
                    store = UserStores[username];
                }
                else
                {
					store = new EventStore();
                    UserStores.Add(username, store);
                }
            }
            finally
            {
                Lock.ExitUpgradeableReadLock();
            }

            return store;
        }

        public static void Increment(DbEventObject eventObject)
        {
            GlobalStore.Increment(eventObject);

            if (!string.IsNullOrEmpty(eventObject.Username))
            {
                EventStore userStore = GetUserStore(eventObject.Username);
                userStore.Increment(eventObject);
            }
        }

        public static DbEventObjectWithCount[] GetGlobalMostFrequent(int howMany)
        {
            DbEventObjectWithCount[] eventObjects = GlobalStore.GetMostFrequent(howMany);

	        if (eventObjects.Length == 0)
	        {
		        return GetInitialButtons();
	        }
			
			return eventObjects;
        }

        public static DbEventObjectWithCount[] GetUserMostFrequent(string userId, int howMany)
        {
            EventStore userStore = GetUserStore(userId);

            DbEventObjectWithCount[] eventObjects = userStore.GetMostFrequent(howMany);
            return eventObjects;
        }

	    private static DbEventObjectWithCount[] GetInitialButtons()
	    {
		    var array = new DbEventObjectWithCount[]
		    {
			    new DbEventObjectWithCount()
			    {
				    Count = 1,
				    Object = new DbEventObject()
				    {
					    Id = 1,
					    ReferenceId = "{14550BAD-AF45-42C9-ADF4-4BED5FA6CB3E}", // Publish button
					    Timestamp = DateTime.Now,
					    Username = "everyone"
				    }
			    },
			    new DbEventObjectWithCount()
			    {
				    Count = 1,
				    Object = new DbEventObject()
				    {
					    Id = 2,
					    ReferenceId = "{AE7CA3FB-770F-43A9-8BD9-B0E67090DD61}", // Experience Editor button
					    Timestamp = DateTime.Now,
					    Username = "everyone"
				    }
			    }
		    };

		    return array;
	    }
    }

    public class DbEventObjectWithCount
    {
        public DbEventObject Object { get; set; }

        public int Count { get; set; }

	    public void Add(DbEventObject dbEventObject)
	    {
		    throw new NotImplementedException();
	    }
    }

    public class EventProcessingTask
    {
        private static readonly object Lock = new object();

        private static DateTime _lastProcessed = DateTime.MinValue;

        private static readonly DbDataService _dataService = new DbDataService();

        public void Run()
        {
            lock (Lock)
            {
                DateTime previousLastProcessed = GetLastProcessedId();

                DbEventObject[] eventsToProcess = GatherEventsToProcess(previousLastProcessed);

                DateTime? lastProcessed = null;

                try
                {
                    foreach (DbEventObject eventObject in eventsToProcess)
                    {
                        EventDataStore.Increment(eventObject);
                        lastProcessed = eventObject.Timestamp;
                    }
                }
                finally
                {
                    if (lastProcessed.HasValue)
                    {
                        SetLastProcessedId(lastProcessed.Value);
                    }
                }
            }
        }

        private DbEventObject[] GatherEventsToProcess(DateTime id)
        {
            return _dataService.GetLastEvents(id).ToArray();
        }

        private DateTime GetLastProcessedId()
        {
            return _lastProcessed;
        }

        private void SetLastProcessedId(DateTime id)
        {
            _lastProcessed = id;
        }
    }
}
