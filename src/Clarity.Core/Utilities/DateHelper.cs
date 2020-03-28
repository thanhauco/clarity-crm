using System;

namespace Clarity.Core.Utilities 
{ 
    public static class DateHelper 
    { 
        public static DateTime GetEndOfMonth(DateTime date) 
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static DateTime GetStartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    } 
}
