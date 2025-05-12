using System;
using System.Globalization; // Needed for DateTime formatting
using System.IO; // Added for file operations
using System.Xml.Linq;

public class Receipt // Renamed from Recipe
{
    public string PhoneNumber { get; set; }
    public string Operator { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string MaskedCardNumber { get; set; } // Storing the already masked card number

    public string ToXmlAndSave()
    {
        XElement receiptXml = new XElement(
            "Receipt",
            new XElement("PhoneNumber", this.PhoneNumber),
            new XElement("Operator", this.Operator),
            // Format Amount consistently, e.g., using InvariantCulture
            new XElement("Amount", this.Amount.ToString(CultureInfo.InvariantCulture)),
            // Format Date consistently, e.g., using ISO 8601 format
            new XElement("Date", this.Date.ToString("o", CultureInfo.InvariantCulture)), // "o" for round-trip format
            new XElement("MaskedCardNumber", this.MaskedCardNumber)
        );

        string xmlString = receiptXml.ToString();

        // Generate filename: e.g., "20231027153000_receipt.xml"
        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_receipt.xml";

        // Save the XML string to the generated file path in the current directory
        File.WriteAllText(fileName, xmlString);

        return fileName; // Return the generated filename
    }
}
