using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class CardService
{
    public List<Card> userCards { get; set; }

    public CardService()
    {
        this.LoadUserCards("cards.json");
    }

    public void LoadUserCards(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        userCards = JsonSerializer.Deserialize<List<Card>>(jsonString);
    }

    public Card FindCard(string cardNumber, int expiryMonth, int expiryYear, string cvv)
    {
        if (!Card.ValidateCardDetails(cardNumber, expiryMonth, expiryYear, cvv))
        {
            return null; // Invalid card details
        }

        foreach (var card in userCards)
        {
            if (
                card.CardNumber == cardNumber
                && card.ExpiryMonth == expiryMonth
                && card.ExpiryYear == expiryYear
                && card.Cvv == cvv
            )
            {
                return card; // Found matching card
            }
        }

        return null; // No matching card found
    }
}
