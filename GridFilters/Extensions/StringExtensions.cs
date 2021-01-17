namespace GridFilters.Extensions
{
    internal static class StringExtensions
    {
        internal static bool IsNullOrEmpty(this object str) => string.IsNullOrEmpty(str?.ToString());

        internal static string ToStringSafe(this object str) => str.IsNullOrEmpty() ? "" : str.ToString();
    }
}
