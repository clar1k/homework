using System; // Add System for DateTime
using System.Collections.Generic;
using System.IO;
using System.Linq; // Add System.Linq for char.IsDigit
using System.Text.Json;

public class Bank
{
    public string Name { get; set; }
    public double Fee { get; set; }
    public List<Card> Cards { get; set; }

    public static List<Bank> LoadBanks(string filePath = "data/banks.json")
    {
        string jsonString = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        List<Bank> banks = JsonSerializer.Deserialize<List<Bank>>(jsonString, options);

        foreach (var bank in banks)
        {
            Logger.Log($"method:LoadBanks Add Bank ${bank.name}");
        }

        return banks ?? new List<Bank>();
    }

    public static Card? FindUserCard(
        List<Bank> banks,
        string cardNumber,
        int expiryMonth,
        int expiryYear,
        string cvv,
        string pinCode
    )
    {
        if (!Card.ValidateCardDetails(cardNumber, expiryMonth, expiryYear, cvv, pinCode))
        {
            Console.WriteLine("Invalid card details provided.");
            return null;
        }

        foreach (var bank in banks)
        {
            bool hasCards = bank.Cards != null;

            if (!hasCards)
            {
                continue;
            }

            foreach (var card in bank.Cards)
            {
                bool isUserCard =
                    card.CardNumber == cardNumber
                    && card.ExpiryMonth == expiryMonth
                    && card.ExpiryYear == expiryYear
                    && card.Cvv == cvv
                    && card.PinCode == pinCode;

                if (isUserCard)
                {
                    return card;
                }
            }
        }

        Console.WriteLine("Card not found.");
        return null;
    }

    public static double GetFee(List<Bank> banks, string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || !cardNumber.All(char.IsDigit))
        {
            Console.WriteLine("Invalid card number format provided.");
            return 0;
        }

        foreach (var bank in banks)
        {
            if (bank.Cards == null)
                continue; // Skip banks with no cards list

            foreach (var card in bank.Cards)
            {
                if (card.CardNumber == cardNumber)
                {
                    return bank.Fee;
                }
            }
        }

        Console.WriteLine($"Card number {cardNumber} not associated with any known bank.");
        return 0; // Card number not found in any bank
    }
}
