using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings
{
    public static class DateTimeExtension
    {
        const int TimeSlippage = 40;

        public static bool IsNotInRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {            
            var isInRange = dateToCheck >= startDate 
                && endDate.AddMinutes(TimeSlippage) <= dateToCheck;
            return !isInRange;
        }
    }
}