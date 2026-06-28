namespace TicketManagement.Server.Helper
{
    public class DateTimeHelper
    {
        // Current UTC date time
        public static DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        // Current local date time (India)
        public static DateTime IndiaNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        // OTP expiry after X minutes
        public static DateTime GetOtpExpiry(int minutes = 10)
        {
            return DateTime.UtcNow.AddMinutes(minutes);
        }

        // Only date
        public static DateOnly Today()
        {
            return DateOnly.FromDateTime(DateTime.UtcNow);
        }

        // Current year
        public static int CurrentYear()
        {
            return DateTime.UtcNow.Year;
        }

        // Current month
        public static int CurrentMonth()
        {
            return DateTime.UtcNow.Month;
        }

        // Current day
        public static int CurrentDay()
        {
            return DateTime.UtcNow.Day;
        }
    }
}
