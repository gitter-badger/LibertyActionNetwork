using System;

namespace Li.Lan.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public const string DateTimeSortableToStringFormat = "yyyyMMddHHmmssfff";

        public const string ShortDateTimeStringFormat = "MM/dd/yyyy hh:mm:ss tt";

        public const string ExportDateTimeStringFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        public static string GetOrdinalString(int i)
        {
            if (i == 1) return "st";
            if (i == 2) return "nd";
            if (i == 3) return "rd";
            return "th";
        }

        /// <summary>
        /// Gets the Week's start date.  Default start DayOfWeek is Sunday.
        /// </summary>
        /// <param name="date">The date to retrieve the Week Start Date of.</param>
        /// <param name="startOfWeek">DayOfWeek that is the first day of the week.</param>
        /// <returns>The Date that represents the Week Start Date at 00:00:00.</returns>
        public static DateTime WeekStartDate(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = date.DayOfWeek - startOfWeek;

            if (diff < 0) diff += 7;

            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the Week's end date.  Default start DayOfWeek is Sunday.
        /// </summary>
        /// <param name="date">The date to retrieve the Week End Date of.</param>
        /// <param name="startOfWeek">DayOfWeek that is the first day of the week.</param>
        /// <returns>The Date that represents the Week End Date at 23:59:59.</returns>
        public static DateTime WeekEndDate(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = date.DayOfWeek - startOfWeek;

            if (diff < 0) diff += 7;

            return date.AddDays((-1 * diff) + 6).Date.EndOfDay();
        }

        /// <summary>
        /// Gets the first day of the month for the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>The first day of the month.</returns>
        public static DateTime MonthStartDate(this DateTime date)
        {
            int month = date.Month;
            int year = date.Year;

            return new DateTime(year, month, 1);
        }

        /// <summary>
        /// Gets the last day of the month for the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>The last day of the month for the given date.</returns>
        public static DateTime MonthEndDate(this DateTime date)
        {
            int month = date.Month;
            int year = date.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            return new DateTime(year, month, daysInMonth).EndOfDay();
        }

        /// <summary>
        /// Gets the quarter of the year for the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>The quarter of the year for the given date.</returns>
        public static int Quarter(this DateTime date)
        {
            if (date.Month <= 3) return 1;
            if (date.Month <= 6) return 2;
            if (date.Month <= 9) return 3;
            return 4;
        }

        public static DateTime QuarterStartDate(this DateTime date)
        {
            int month = ((date.Quarter() - 1) * 3) + 1;
            int year = date.Year;

            return new DateTime(year, month, 1);
        }

        public static DateTime QuarterEndDate(this DateTime date)
        {
            int month = date.Quarter() * 3;
            int year = date.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            return new DateTime(year, month, daysInMonth).EndOfDay();
        }

        public static DateTime YearStartDate(this DateTime date)
        {
            int year = date.Year;

            return new DateTime(year, 1, 1);
        }

        public static DateTime YearEndDate(this DateTime date)
        {
            int year = date.Year;

            return new DateTime(year, 12, 31).EndOfDay();
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.AddDays(1).Date.AddTicks(-1);
        }

        public static string ToStringSortable(this DateTime date)
        {
            return date.ToString(DateTimeExtensions.DateTimeSortableToStringFormat);
        }

        public static string ToShortDateTimeString(this DateTime date)
        {
            return date.ToString(ShortDateTimeStringFormat);
        }

        public static string ToShortDateTimeString(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(ShortDateTimeStringFormat);

            return "";
        }

        public static bool IsWithinPastHour(this DateTime date)
        {
            return IsWithinPastHour(date, DateTime.UtcNow);
        }

        public static bool IsWithinPastHour(this DateTime date, DateTime now)
        {
            return IsWithinPastTimeSpan(date, TimeSpan.FromHours(1), now);
        }

        public static bool IsWithinPastTimeSpan(this DateTime date, TimeSpan timeSpan)
        {
            return IsWithinPastTimeSpan(date, timeSpan, DateTime.UtcNow);
        }

        public static bool IsWithinPastTimeSpan(this DateTime date, TimeSpan timeSpan, DateTime now)
        {
            return IsDateTimeWithinRange(date, now.Add(-timeSpan.Duration()), now);
        }

        /// <summary>
        /// Determines whether a given DateTime is within the given Start and End dates.
        /// The bounds are inclusive ("[greater|less] than or equal to").
        /// The dateTimeToTest must be
        /// greater than or equal to the startDateTime
        /// and also
        /// less than or equal to the endDateTime.
        /// </summary>
        /// <param name="dateTimeToTest"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns>True if the dateTimeToTest is within the dateTimeStart and dateTimeEnd, inclusive. Otherwise False.</returns>
        public static bool IsDateTimeWithinRange(DateTime dateTimeToTest, DateTime startDateTime, DateTime endDateTime)
        {
            return
                startDateTime <= dateTimeToTest
                && dateTimeToTest <= endDateTime;
        }
    }
}