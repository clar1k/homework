using System.IO;

public static class Logger
{
    private static readonly string logFilePath = "log.txt";
    private static readonly object _lock = new object();

    public static void Log(string message)
    {
        try
        {
            lock (_lock)
            {
                File.AppendAllText(
                    logFilePath,
                    $"{System.DateTime.Now}: {message}{System.Environment.NewLine}"
                );
            }
        }
        catch (System.Exception ex)
        {
            // Handle potential exceptions during file writing (e.g., permissions)
            System.Console.WriteLine($"Error writing to log file: {ex.Message}");
            // Optionally, re-throw or log the error somewhere else
        }
    }
}
