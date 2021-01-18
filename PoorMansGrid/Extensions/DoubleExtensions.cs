namespace PoorMansGrid.Extensions
{
    internal static class DoubleExtensions
    {
        internal static double? ToDouble(this object number)
        {
            if (double.TryParse(number.ToStringSafe(), out var result)) return result;
            return null;
        }
    }
}
