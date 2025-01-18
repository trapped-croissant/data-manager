namespace Carr.DataGenerator.Extensions;

public static class RandomExtensions
{
    public static int NextInt(this Random random, int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

    public static string NextString(this Random random, int maxLength)
    {
        var maxStringLength = random.Next(maxLength);
        return new string(Enumerable.Repeat(Chars, maxStringLength).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static bool NextBool(this Random random)
    {
        return random.Next(2) == 1;
    }

    public static decimal NextDecimal(this Random random, int minValue, int maxValue)
    {
        var defaultIntValue = random.Next(minValue, maxValue);
        var decimalFactor = (decimal)Math.Pow(10, random.Next(10));
        return defaultIntValue / decimalFactor + minValue;
    }

    public static DateTime NextDateTime(this Random random, DateTime minValue, DateTime maxValue)
    {
        var range = (maxValue - minValue).Days;           
        return minValue.AddDays(random.Next(range));
    }
}