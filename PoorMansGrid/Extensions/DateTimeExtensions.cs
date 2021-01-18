using System;

namespace PoorMansGrid.Extensions
{
    internal static class DateTimeExtensions
    {
        internal static DateTime? ToDateTime(this object date)
        {
            if (DateTime.TryParse(date.ToStringSafe(), out var result)) return result;
            return null;
        }
    }
}
