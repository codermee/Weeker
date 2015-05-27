using System;
using System.Globalization;

namespace ResourceLib
{
    public class Helper
    {
        public static int GetWeekNumber(DateTime date)
        {
            int currentWeek;

            // Get jan 1st of the year
            var startOfYear = date.AddDays(-date.Day + 1).AddMonths(-date.Month + 1);

            // Get dec 31st of the year
            var endOfYear = startOfYear.AddYears(1).AddDays(-1);

            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // DayOfWeek returns 0 for sunday up to 6 for saturday
            int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
            var nds = date.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
            var weekNumber = nds / 7;
            switch (weekNumber)
            {
                case 0:
                    // Return weeknumber of dec 31st of the previous year
                    currentWeek = GetWeekNumber(startOfYear.AddDays(-1));
                    break;
                case 53:
                    // If dec 31st falls before thursday it is week 01 of next year
                    currentWeek = endOfYear.DayOfWeek < DayOfWeek.Thursday ? 1 : weekNumber;
                    break;
                default:
                    currentWeek = weekNumber;
                    break;
            }
            return currentWeek;
        }

        public static string GetFriendlyDateRange(DateTime date)
        {
            var dayOfWeek = Convert.ToInt32(date.DayOfWeek);
            DateTime weekStartDate;
            DateTime weekEndDate;
            string dateRange;

            var currentCulture = CultureInfo.CurrentUICulture;
            if (currentCulture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday)
            {
                if (dayOfWeek == 0) // 0 is sunday which returns next week´s date range so it needs to be set to 7
                {
                    dayOfWeek = 7;
                }
                weekStartDate = date.AddDays(1 - dayOfWeek);
                weekEndDate = date.AddDays(7 - dayOfWeek);
            }
            else
            {
                weekStartDate = date.AddDays(0 - dayOfWeek);
                weekEndDate = date.AddDays(6 - dayOfWeek);
            }

            switch (currentCulture.ToString())
            {
                case "en-US":
                    dateRange = String.Format("{0} {1} - {2} {3}", currentCulture.DateTimeFormat.GetMonthName(weekStartDate.Month).Substring(0, 3).ToUpper(),
                                                                   weekStartDate.Day,
                                                                   currentCulture.DateTimeFormat.GetMonthName(weekEndDate.Month).Substring(0, 3).ToUpper(),
                                                                   weekEndDate.Day);
                    break;
                default:
                    dateRange = String.Format("{0} {1} - {2} {3}", weekStartDate.Day,
                                                                   currentCulture.DateTimeFormat.GetMonthName(weekStartDate.Month).Substring(0, 3).ToUpper(),
                                                                   weekEndDate.Day,
                                                                   currentCulture.DateTimeFormat.GetMonthName(weekEndDate.Month).Substring(0, 3).ToUpper());
                    break;
            }
            return dateRange;
        }

        public static string GetFriendlyDate(DateTime date)
        {
            var month = CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(date.Month);
            string friendlyDate;
            switch (CultureInfo.CurrentUICulture.ToString())
            {
                case "en-US":
                    friendlyDate = String.Format("{0} {1}", month.Substring(0, 3).ToUpper(), date.Day);
                    break;
                default:
                    friendlyDate = String.Format("{0} {1}", date.Day, month.Substring(0, 3).ToUpper());
                    break;
            }
            return friendlyDate;
        }

        public static string GetDayOfWeek(DateTime date)
        {
            return CultureInfo.CurrentUICulture.DateTimeFormat.GetDayName(date.DayOfWeek).ToUpper();
        }

        public static string GetDateRangeByWeek(int year, int week)
        {
            var date = new DateTime(year, 1, 1);

            // multiply by 7 to get the day number of the year
            var days = (week - 1) * 7;

            // get the date of that day
            var dateTime = date.AddDays(days);

            // get the actual weeknumber of that date (to make sure it´s a valid combo of year and week selected)
            var wk = GetWeekNumber(dateTime);

            var returnValue = wk != week ? Resource.ErrorMessageWeekConvertion : GetFriendlyDateRange(dateTime);

            return returnValue;
        }
    }
}