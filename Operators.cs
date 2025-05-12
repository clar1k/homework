using System;
using System.IO;
using System.Linq;

public class Operators
{
    public string[] CurrentOperators { get; private set; }
    public string[] AccesibleCodes { get; private set; }

    public void LoadOperatorsFromFile(string filePath = "./data/operators.txt")
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            CurrentOperators = lines.Select(line => line.Split(':')[0]).ToArray();
            AccesibleCodes = lines.Select(line => line.Split(':')[1]).ToArray();
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

    public bool ValidatePhoneNumber(string phoneNumber)
    {
        if (AccesibleCodes == null || AccesibleCodes.Length == 0 || string.IsNullOrEmpty(phoneNumber))
        {
            // Or potentially log a warning/throw an exception if codes haven't been loaded
            return false;
        }

        foreach (var code in AccesibleCodes)
        {
            if (phoneNumber.StartsWith(code))
            {
                return true;
            }
        }

        return false;
    }
}
