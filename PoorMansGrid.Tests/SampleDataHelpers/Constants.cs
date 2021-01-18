using System;

namespace PoorMansGrid.Tests.SampleDataHelpers
{
    public class Constants
    {
        public static DateTime LastYear => DateTime.Today.AddYears(-1);
        public static DateTime LastMonth => DateTime.Today.AddMonths(-1);
        public static DateTime Yesterday => DateTime.Today.AddDays(-1);

        public static DateTime Today => DateTime.Today;


        public static DateTime Tomorrow => DateTime.Today.AddDays(1);
        public static DateTime NextMonth => DateTime.Today.AddMonths(1);
        public static DateTime NextYear => DateTime.Today.AddYears(1);
    }
}
