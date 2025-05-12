using System;
using System.IO;
using System.Linq;

public class Operators
{
    public string[] CurrentOperators { get; private set; }

    public void LoadOperatorsFromFile(string filePath = "./data/operators.txt")
    {
        try
        {
            CurrentOperators = File.ReadAllLines(filePath);
        }
        catch (FileNotFoundException)
        {
            Logger.Log($"Error: The file '{filePath}' was not found.");
            CurrentOperators = Array.Empty<string>();
        }
        catch (Exception exception)
        {
            Logger.Log($"An error occurred while reading the file: {exception.Message}");
            CurrentOperators = Array.Empty<string>();
        }
    }
}
