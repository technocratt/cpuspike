using System.Security.Cryptography;
using System.Text;

namespace cpuspike;

public class CPUStressTest
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int stringLength = 1000;
    private const int hashIterations = 1000;
    private const int processIterations = 1000;

    public static void RunStressTest()
    {
        Parallel.For(0, Environment.ProcessorCount, pid =>
        {
            for (int i = 0; i < processIterations; i++)
                PerformHashingOperations(GenerateRandomString(stringLength));
        });
    }

    private static string GenerateRandomString(int length)
    {
        Random random = new();
        StringBuilder sb = new(length);

        for (int i = 0; i < length; i++)
            sb.Append(chars[random.Next(chars.Length)]);

        return sb.ToString();
    }

    private static void PerformHashingOperations(string input)
    {
        var hash = Encoding.UTF8.GetBytes(input);
        for (int i = 0; i < hashIterations; i++)
            hash = SHA512.HashData(hash);
    }
}