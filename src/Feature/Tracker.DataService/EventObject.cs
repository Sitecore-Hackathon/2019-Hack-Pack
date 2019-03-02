using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.DataService
{
    public class EventObject
    {
        public string header { get; set; }
        public string click { get; set; }
        public string chunk { get; set; }
        public string icon { get; set; }
        public string strip { get; set; }
        public string tooltip { get; set; }
    }

    public class DbEventObject
    {
        public int Id { get; set; }

        public string ReferenceId { get; set; }

        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
    }
}
