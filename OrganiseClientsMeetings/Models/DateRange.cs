using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class DateRange
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateRange(DateTime startTime, DateTime endTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }
}