using System;
using System.Linq;

public class Card
{
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Cvv { get; set; }
    public int Balance { get; set; }
    public string PinCode { get; set; }

    public Card(string cardNumber, int expiryMonth, int expiryYear, string cvv, string pinCode)
    {
        CardNumber = cardNumber;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Cvv = cvv;
        Balance = 0;
        PinCode = pinCode;
    }

    public static bool ValidateCardDetails(
        string cardNumber,
        int expiryMonth,
        int expiryYear,
        string cvv,
        string pinCode
    )
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || !cardNumber.All(char.IsDigit))
        {
            return false;
        }

        if (expiryMonth < 1 || expiryMonth > 12)
        {
            return false;
        }

        // Simple year validation (e.g., must be current year or later)
        // More precise check: compare month/year against current date
        int currentYear = DateTime.Now.Year % 100; // Assuming 2-digit year input
        int currentMonth = DateTime.Now.Month;

        // Handle century ambiguity if needed, assuming year is YY format for now
        // A full 4-digit year input (YYYY) would simplify this.
        // If year is 2 digits, assume 20xx
        int fullExpiryYear = expiryYear + 2000; // Adjust if needed

        if (
            fullExpiryYear < DateTime.Now.Year
            || (fullExpiryYear == DateTime.Now.Year && expiryMonth < currentMonth)
        )
        {
            return false; // Card has expired
        }

        if (
            string.IsNullOrWhiteSpace(cvv)
            || !cvv.All(char.IsDigit)
            || (cvv.Length != 3 && cvv.Length != 4)
        )
        {
            return false;
        }

        // Added basic PIN validation (can be expanded)
        if (string.IsNullOrWhiteSpace(pinCode) || !pinCode.All(char.IsDigit) || pinCode.Length != 4) // Example: 4 digits
        {
            return false;
        }

        return true;
    }
}
