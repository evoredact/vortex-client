using System.Linq;

namespace VortexClient.Utils;

internal class Random
{
    private static System.Random random = new();
    public static string RandomString(int length = 11) {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}