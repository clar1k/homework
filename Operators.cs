using System;
using System.IO;
using System.Linq;

public class Operators
{
    public string[] CurrentOperators { get; private set; }

    public void LoadOperatorsFromFile(string filePath)
    {
        try
        {
            CurrentOperators = File.ReadAllLines(filePath);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The file '{filePath}' was not found.");
            CurrentOperators = Array.Empty<string>(); // Initialize with an empty array if file not found
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An error occurred while reading the file: {exception.Message}");
            CurrentOperators = Array.Empty<string>(); // Initialize with an empty array on other errors
        }
    }
}
