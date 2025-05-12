using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Operators
{
    public string[] CurrentOperators { get; private set; }
    public string[] AccesibleCodes { get; private set; }
    public Dictionary<string, string> OperatorToCodeMap { get; private set; }

    public void LoadOperatorsFromFile(string filePath = "./data/operators.txt")
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            CurrentOperators = lines.Select(line => line.Split(':')[0]).ToArray();
            AccesibleCodes = lines.Select(line => line.Split(':')[1]).ToArray();
            OperatorToCodeMap = lines.ToDictionary(line => line.Split(':')[0], line => line.Split(':')[1]);
        }
        catch (FileNotFoundException)
        {
            Logger.Log($"Error: The file '{filePath}' was not found.");
            CurrentOperators = Array.Empty<string>();
            AccesibleCodes = Array.Empty<string>();
            OperatorToCodeMap = new Dictionary<string, string>();
        }
        catch (Exception exception)
        {
            Logger.Log($"An error occurred while reading the file: {exception.Message}");
            CurrentOperators = Array.Empty<string>();
            AccesibleCodes = Array.Empty<string>();
            OperatorToCodeMap = new Dictionary<string, string>();
        }
    }

    public bool ValidatePhoneNumber(string phoneNumber, string mobOperator)
    {
        if (AccesibleCodes == null || AccesibleCodes.Length == 0 || string.IsNullOrEmpty(phoneNumber))
        {
            // Or potentially log a warning/throw an exception if codes haven't been loaded
            return false;
        }

        var code = OperatorToCodeMap[mobOperator];
        
        if(code == null) {
            return false;
        }

        var isValid = phoneNumber.StartsWith(code);

        return isValid;
    }
}
