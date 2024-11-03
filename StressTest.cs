using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace cpuspike;

public class CPUStressTest
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int stringLength = 100_000;
    private const int hashIterations = 100_000;
    private const int testRunDurationInMinutes = 120;

    public static void RunStressTest()
    {
        PrintEnvironmentDetails();
        var target = DateTime.Now.AddMinutes(testRunDurationInMinutes);

        Console.WriteLine("Stress test started.");
        var startTime = Stopwatch.GetTimestamp();

        while (DateTime.Now < target)
            Parallel.For(0, Environment.ProcessorCount, pid => PerformHashingOperations(GenerateRandomString(stringLength)));

        var duration = Stopwatch.GetElapsedTime(startTime);
        Console.WriteLine($"Stress test completed in {duration}.");
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

    private static void PrintEnvironmentDetails()
    {
        var containerFilePath = "/proc/1/cpuset";
        var containerId = File.Exists(containerFilePath)
                            ? File.ReadAllText(containerFilePath).Split('/').Last().Trim()
                            : "NA";

        Console.WriteLine($"Operating System: {Environment.OSVersion}");
        Console.WriteLine($"Machine Name: {Environment.MachineName}");
        Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");
        Console.WriteLine($"Container ID: {containerId}");
    }
}