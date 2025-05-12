using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace hack
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Dictionary<string, string>> languageStrings =
            new Dictionary<string, Dictionary<string, string>>();

        // List to store user data
        private List<UserData> userData = new List<UserData>();
        private Operators operators = new Operators();
        private List<Bank> banks = new List<Bank>();

        // Class to store user data
        private class UserData
        {
            public string PhoneNumber { get; set; }
            public string Operator { get; set; }
            public DateTime RegistrationDate { get; set; }

            public UserData(string phoneNumber, string op)
            {
                PhoneNumber = phoneNumber;
                Operator = op;
                RegistrationDate = DateTime.Now;
            }
        }

        public Form1()
        {
            InitializeComponent();
            InitializeLanguageStrings();
            operators.LoadOperatorsFromFile("./data/operators.txt");
            banks = Bank.LoadBanks();
            SetupLanguagePicker();
        }

        private void SetupLanguagePicker()
        {
            // Create a label for the language picker
            Label lblLanguage = new Label();
            lblLanguage.Location = new Point(20, 20);
            lblLanguage.Size = new Size(80, 20);
            lblLanguage.Text = "Language:";

            // Create the language combo box
            ComboBox cboLanguage = new ComboBox();
            cboLanguage.Location = new Point(100, 20);
            cboLanguage.Size = new Size(150, 25);
            cboLanguage.Name = "cboLanguage";
            cboLanguage.DropDownStyle = ComboBoxStyle.DropDownList;

            // Add language options
            cboLanguage.Items.Add("English");
            cboLanguage.Items.Add("Українська");
            cboLanguage.SelectedIndex = 0; // Default to English

            // Add event handler for language change - fix nullability with null-forgiving operator
            cboLanguage.SelectedIndexChanged += CboLanguage_SelectedIndexChanged!;

            // Add controls to form
            this.Controls.Add(lblLanguage);
            this.Controls.Add(cboLanguage);

            // Add example controls to demonstrate localization
            AddExampleControls();
        }

        private void InitializeLanguageStrings()
        {
            // Initialize dictionary with UI strings for different languages
            // English strings
            var englishStrings = new Dictionary<string, string>
            {
                { "lblWelcome", "Welcome to the application!" },
                { "btnSubmit", "Submit" },
                { "phoneLabel", "Enter phone number:" },
                { "invalidPhone", "Please enter a valid phone number" },
                { "successRegistration", "Registration successful!" },
                { "lblCardNumber", "Card Number:" },
                { "lblExpiryMonth", "Expiry Month (MM):" },
                { "lblExpiryYear", "Expiry Year (YY):" },
                { "lblCvv", "CVV:" },
                { "invalidCardDetails", "Invalid card details provided." },
                { "cardDetailsSuccess", "Card details accepted." },
                { "lblDepositAmount", "Deposit Amount:" },
                { "btnMainSubmitText", "Finalize Payment" },
                { "receiptSavedSuccessMessage", "Receipt saved successfully: {0}" },
                { "mainSubmitErrorTitle", "Input Error" },
                { "phoneEmptyError", "Phone number cannot be empty or is invalid." },
                { "operatorNotSelectedError", "Please select a mobile operator." },
                { "amountInvalidError", "Please enter a valid deposit amount." },
                { "cardNumberEmptyError", "Card number cannot be empty." },
                { "chkPrintReceiptText", "Print the check" },
                { "lblPinCode", "PIN Code:" },
                { "lblCharityPercent", "Charity Donation (%):" },
                { "feeWarningTitle", "Fee Warning" },
                { "feeWarningMessage", "The fee for this transaction is {0}%." },
            };
            languageStrings["English"] = englishStrings;

            // Ukrainian strings
            var ukrainianStrings = new Dictionary<string, string>
            {
                { "lblWelcome", "Ласкаво просимо до програми!" },
                { "btnSubmit", "Підтвердити" },
                { "phoneLabel", "Введіть номер телефону:" },
                { "invalidPhone", "Будь ласка, введіть правильний номер телефону" },
                { "successRegistration", "Реєстрація успішна!" },
                { "lblCardNumber", "Номер картки:" },
                { "lblExpiryMonth", "Місяць (MM):" },
                { "lblExpiryYear", "Рік (YY):" },
                { "lblCvv", "CVV:" },
                { "invalidCardDetails", "Недійсні дані картки." },
                { "cardDetailsSuccess", "Дані картки прийнято." },
                { "lblDepositAmount", "Сума поповнення:" },
                { "btnMainSubmitText", "Завершити платіж та зберегти чек" },
                { "receiptSavedSuccessMessage", "Чек успішно збережено: {0}" },
                { "mainSubmitErrorTitle", "Помилка введення" },
                { "phoneEmptyError", "Номер телефону не може бути порожнім або недійсний." },
                { "operatorNotSelectedError", "Будь ласка, оберіть мобільного оператора." },
                { "amountInvalidError", "Будь ласка, введіть дійсну суму поповнення." },
                { "cardNumberEmptyError", "Номер картки не може бути порожнім." },
                { "chkPrintReceiptText", "Роздрукувати чек" },
                { "lblPinCode", "PIN-код:" },
                { "lblCharityPercent", "Відсоток на благодійність (%):" },
                { "feeWarningTitle", "Попередження про оплату" },
                { "feeWarningMessage", "Оплата за цю транзакцію становить {0}%." },
            };
            languageStrings["Українська"] = ukrainianStrings;
        }

        private void AddExampleControls()
        {
            // Add a welcome label
            Label lblWelcome = new Label();
            lblWelcome.Location = new Point(20, 60);
            lblWelcome.Size = new Size(300, 20);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Text = languageStrings["English"]["lblWelcome"];

            // Add mobile operator selection
            Label lblOperator = new Label();
            lblOperator.Location = new Point(20, 100);
            lblOperator.Size = new Size(150, 20);
            lblOperator.Text = "Mobile Operator:";
            lblOperator.Name = "lblOperator";

            ComboBox cboOperator = new ComboBox();
            cboOperator.Location = new Point(20, 120);
            cboOperator.Size = new Size(200, 25);
            cboOperator.Name = "cboOperator";
            cboOperator.DropDownStyle = ComboBoxStyle.DropDownList;

            // Add operator options
            cboOperator.Items.Add("PeregryadokCell");
            cboOperator.Items.Add("KrisMobile");
            cboOperator.Items.Add("Durkilfone");

            // Clear existing items
            cboOperator.Items.Clear();

            foreach (string operatorName in operators.CurrentOperators)
            {
                cboOperator.Items.Add(operatorName);
            }

            cboOperator.SelectedIndex = 0; // Default to first operator

            // Add phone number input field with label
            Label lblPhone = new Label();
            lblPhone.Location = new Point(20, 160);
            lblPhone.Size = new Size(150, 20);
            lblPhone.Name = "lblPhone";
            lblPhone.Text = languageStrings["English"]["phoneLabel"];

            TextBox txtPhone = new TextBox();
            txtPhone.Location = new Point(20, 180);
            txtPhone.Size = new Size(200, 25);
            txtPhone.Name = "txtPhone";

            // Add operator submit button (Moved slightly)
            Button btnOperatorSubmit = new Button();
            btnOperatorSubmit.Location = new Point(230, 180); // Adjusted position
            btnOperatorSubmit.Size = new Size(150, 30);
            btnOperatorSubmit.Name = "btnOperatorSubmit";
            btnOperatorSubmit.Text = "Submit Operator";
            btnOperatorSubmit.Click += HandleOperatorSubmit!;

            // --- Card Details Input ---
            int cardInputYStart = 220;
            int currentCardY = cardInputYStart; // Variable to track Y position for card details

            // Card Number
            Label lblCardNumber = new Label();
            lblCardNumber.Location = new Point(20, currentCardY);
            lblCardNumber.Size = new Size(150, 20);
            lblCardNumber.Name = "lblCardNumber";
            lblCardNumber.Text = languageStrings["English"]["lblCardNumber"];
            this.Controls.Add(lblCardNumber);

            currentCardY += lblCardNumber.Height + 5; // Add padding

            TextBox txtCardNumber = new TextBox();
            txtCardNumber.Location = new Point(20, currentCardY);
            txtCardNumber.Size = new Size(200, 25);
            txtCardNumber.Name = "txtCardNumber";
            this.Controls.Add(txtCardNumber);

            currentCardY += txtCardNumber.Height + 15; // Add more padding before next section

            // Expiry Month
            Label lblExpiryMonth = new Label();
            lblExpiryMonth.Location = new Point(20, currentCardY);
            lblExpiryMonth.Size = new Size(150, 20); // Adjusted size for consistency
            lblExpiryMonth.Name = "lblExpiryMonth";
            lblExpiryMonth.Text = languageStrings["English"]["lblExpiryMonth"];
            this.Controls.Add(lblExpiryMonth);

            currentCardY += lblExpiryMonth.Height + 5;

            TextBox txtExpiryMonth = new TextBox();
            txtExpiryMonth.Location = new Point(20, currentCardY);
            txtExpiryMonth.Size = new Size(200, 25); // Adjusted size
            txtExpiryMonth.Name = "txtExpiryMonth";
            txtExpiryMonth.MaxLength = 2;
            this.Controls.Add(txtExpiryMonth);

            currentCardY += txtExpiryMonth.Height + 15;

            // Expiry Year
            Label lblExpiryYear = new Label();
            lblExpiryYear.Location = new Point(20, currentCardY);
            lblExpiryYear.Size = new Size(150, 20); // Adjusted size for consistency
            lblExpiryYear.Name = "lblExpiryYear";
            lblExpiryYear.Text = languageStrings["English"]["lblExpiryYear"];
            this.Controls.Add(lblExpiryYear);

            currentCardY += lblExpiryYear.Height + 5;

            TextBox txtExpiryYear = new TextBox();
            txtExpiryYear.Location = new Point(20, currentCardY);
            txtExpiryYear.Size = new Size(200, 25); // Adjusted size
            txtExpiryYear.Name = "txtExpiryYear";
            txtExpiryYear.MaxLength = 2;
            this.Controls.Add(txtExpiryYear);

            currentCardY += txtExpiryYear.Height + 15;

            // CVV
            Label lblCvv = new Label();
            lblCvv.Location = new Point(20, currentCardY);
            lblCvv.Size = new Size(150, 20); // Adjusted size for consistency
            lblCvv.Name = "lblCvv";
            lblCvv.Text = languageStrings["English"]["lblCvv"];
            this.Controls.Add(lblCvv);

            currentCardY += lblCvv.Height + 5;

            TextBox txtCvv = new TextBox();
            txtCvv.Location = new Point(20, currentCardY);
            txtCvv.Size = new Size(200, 25); // Adjusted size
            txtCvv.Name = "txtCvv";
            txtCvv.PasswordChar = '*'; // Mask CVV input
            txtCvv.MaxLength = 4;
            this.Controls.Add(txtCvv);

            currentCardY += txtCvv.Height + 15; // Add padding before next section // Adjusted padding

            // PIN Code
            Label lblPinCode = new Label();
            lblPinCode.Location = new Point(20, currentCardY);
            lblPinCode.Size = new Size(150, 20);
            lblPinCode.Name = "lblPinCode";
            lblPinCode.Text = languageStrings["English"]["lblPinCode"]; // Use localized string
            this.Controls.Add(lblPinCode);

            currentCardY += lblPinCode.Height + 5;

            TextBox txtPinCode = new TextBox();
            txtPinCode.Location = new Point(20, currentCardY);
            txtPinCode.Size = new Size(200, 25);
            txtPinCode.Name = "txtPinCode";
            txtPinCode.PasswordChar = '*'; // Mask PIN input
            txtPinCode.MaxLength = 4; // Assuming a 4-digit PIN, adjust as needed
            this.Controls.Add(txtPinCode);

            currentCardY += txtPinCode.Height + 20; // Padding before button

            // Card Submit Button
            Button btnCardSubmit = new Button();
            btnCardSubmit.Location = new Point(20, currentCardY);
            btnCardSubmit.Size = new Size(200, 30);
            btnCardSubmit.Name = "btnCardSubmit";
            btnCardSubmit.Text = languageStrings["English"]["btnSubmit"] + " Card Details";
            btnCardSubmit.Click += HandleCardSubmit!; // Add handler
            this.Controls.Add(btnCardSubmit);

            currentCardY += btnCardSubmit.Height + 20; // Padding after button

            // --- Deposit Amount Input ---
            int depositInputYStart = currentCardY; // Position after card section

            Label lblDepositAmount = new Label();
            lblDepositAmount.Location = new Point(20, depositInputYStart);
            lblDepositAmount.Size = new Size(150, 20);
            lblDepositAmount.Name = "lblDepositAmount";
            lblDepositAmount.Text = languageStrings["English"]["lblDepositAmount"];

            TextBox txtDepositAmount = new TextBox();
            txtDepositAmount.Location = new Point(20, depositInputYStart + 20);
            txtDepositAmount.Size = new Size(100, 25); // Smaller size
            txtDepositAmount.Name = "txtDepositAmount";

            // --- Charity Percentage Input ---
            int charityInputYStart =
                depositInputYStart + lblDepositAmount.Height + txtDepositAmount.Height + 20; // Position below deposit amount, added more padding

            Label lblCharityPercent = new Label();
            lblCharityPercent.Location = new Point(20, charityInputYStart);
            lblCharityPercent.Size = new Size(170, 20); // Adjusted size to fit text
            lblCharityPercent.Name = "lblCharityPercent";
            lblCharityPercent.Text = languageStrings["English"]["lblCharityPercent"];
            this.Controls.Add(lblCharityPercent);

            NumericUpDown nudCharityPercent = new NumericUpDown();
            nudCharityPercent.Location = new Point(200, charityInputYStart - 2); // Align with label, slightly up
            nudCharityPercent.Size = new Size(60, 25);
            nudCharityPercent.Name = "nudCharityPercent";
            nudCharityPercent.Minimum = 0;
            nudCharityPercent.Maximum = 100;
            nudCharityPercent.DecimalPlaces = 0;
            nudCharityPercent.Value = 0; // Default to 0
            this.Controls.Add(nudCharityPercent);

            // --- Print Check Checkbox ---
            int printCheckYStart = charityInputYStart + lblCharityPercent.Height + 15; // Position below charity input, added padding

            CheckBox chkPrintReceipt = new CheckBox();
            chkPrintReceipt.Location = new Point(20, printCheckYStart);
            chkPrintReceipt.Size = new Size(200, 25);
            chkPrintReceipt.Name = "chkPrintReceipt";
            chkPrintReceipt.Text = languageStrings["English"]["chkPrintReceiptText"];
            chkPrintReceipt.Checked = true; // Default to checked
            this.Controls.Add(chkPrintReceipt);

            // --- Main Submit Button ---
            int mainSubmitButtonYStart = printCheckYStart + chkPrintReceipt.Height + 10; // Position below checkbox

            Button btnMainSubmit = new Button();
            btnMainSubmit.Location = new Point(20, mainSubmitButtonYStart);
            btnMainSubmit.Size = new Size(360, 30); // Made wider to fit text
            btnMainSubmit.Name = "btnMainSubmit";
            btnMainSubmit.Text = languageStrings["English"]["btnMainSubmitText"];
            btnMainSubmit.Click += HandleMainSubmit!;
            this.Controls.Add(btnMainSubmit);

            // Remove the old btnLanguageSubmit as it seems redundant now
            var btnLangSubmit = Controls["btnLanguageSubmit"];
            if (btnLangSubmit != null)
            {
                this.Controls.Remove(btnLangSubmit);
                btnLangSubmit.Dispose();
            }
        }

        private void CboLanguage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Get the selected language
            if (sender is ComboBox cbo && cbo.SelectedItem is string selectedLanguage)
            {
                // Update UI with selected language
                UpdateUILanguage(selectedLanguage);
                Logger.Log($"Language changed to: {selectedLanguage}"); // Log language change
            }
        }

        private void HandleLanguageSubmit(object? sender, EventArgs e)
        {
            // Find the language combobox
            var languageControl = Controls["cboLanguage"];
            if (
                languageControl is ComboBox cboLanguage
                && cboLanguage.SelectedItem is string currentLanguage
            )
            {
                // At this point, we might just want to confirm the language change,
                // or perhaps display a generic welcome message if we don't have other specific actions for this button.
                if (
                    languageStrings.ContainsKey(currentLanguage)
                    && languageStrings[currentLanguage].ContainsKey("lblWelcome")
                )
                {
                    MessageBox.Show(languageStrings[currentLanguage]["lblWelcome"]);
                }
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Simple validation for Ukrainian phone numbers
            // Accepts formats: +380xxxxxxxxx or 0xxxxxxxxx
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^(\+380|0)\d{9}$");
        }

        private void HandleOperatorSubmit(object? sender, EventArgs e)
        {
            // Find the operator combobox
            var operatorControl = Controls["cboOperator"];
            if (
                operatorControl is ComboBox cboOperator
                && cboOperator.SelectedItem is string selectedOperator
            )
            {
                // Find the phone textbox
                var phoneControl = Controls["txtPhone"];
                if (phoneControl is TextBox txtPhone)
                {
                    string phoneNumber = txtPhone.Text;

                    // Validate phone number
                    if (!IsValidPhoneNumber(phoneNumber))
                    {
                        var englishLanguage = "English";
                        MessageBox.Show(languageStrings[englishLanguage]["invalidPhone"]);
                        Logger.Log($"Invalid phone number entered: {phoneNumber}"); // Log invalid phone
                        return;
                    }

                    // Store user data
                    userData.Add(new UserData(phoneNumber, selectedOperator));
                    Logger.Log(
                        $"Operator submitted. Phone: {phoneNumber}, Operator: {selectedOperator}"
                    ); // Log successful submission

                    var languageControl = Controls["cboLanguage"];
                    string currentLanguage = "English";
                    if (
                        languageControl is ComboBox cboLanguage
                        && cboLanguage.SelectedItem is string lang
                    )
                    {
                        currentLanguage = lang;
                    }

                    if (languageStrings.ContainsKey(currentLanguage))
                    {
                        MessageBox.Show(
                            languageStrings[currentLanguage]["successRegistration"]
                                + "\n"
                                + "Phone: "
                                + phoneNumber
                                + "\n"
                                + "Operator: "
                                + selectedOperator
                        );
                    }
                }
            }
        }

        private void HandleCardSubmit(object? sender, EventArgs e)
        {
            // Find controls
            var txtCardNumber = Controls["txtCardNumber"] as TextBox;
            var txtExpiryMonth = Controls["txtExpiryMonth"] as TextBox;
            var txtExpiryYear = Controls["txtExpiryYear"] as TextBox;
            var txtCvv = Controls["txtCvv"] as TextBox;
            var txtPinCode = Controls["txtPinCode"] as TextBox; // Find PIN TextBox
            var cboLanguage = Controls["cboLanguage"] as ComboBox;

            string currentLanguage = (cboLanguage?.SelectedItem as string) ?? "English";

            if (
                txtCardNumber == null
                || txtExpiryMonth == null
                || txtExpiryYear == null
                || txtCvv == null
                || txtPinCode == null // Check if PIN TextBox exists
            )
            {
                MessageBox.Show("Error finding card input controls."); // Should not happen
                return;
            }

            string pinCode = txtPinCode.Text; // Get PIN code

            // Declare variables before the if statement
            int expiryMonth;
            int expiryYear;

            // Basic validation: Check if month/year are integers and PIN is valid (basic example)
            if (
                !int.TryParse(txtExpiryMonth.Text, out expiryMonth) // Use pre-declared variable
                || !int.TryParse(txtExpiryYear.Text, out expiryYear) // Use pre-declared variable
                // Basic PIN validation (e.g., not empty, numeric)
                || string.IsNullOrWhiteSpace(pinCode)
                || !pinCode.All(char.IsDigit)
            )
            {
                MessageBox.Show(languageStrings[currentLanguage]["invalidCardDetails"]);
                Logger.Log("Card submission failed: Invalid expiry format or PIN."); // Log format/PIN error
                return;
            }

            // Use the static validation method from Card class, now including PIN
            bool isValid = Card.ValidateCardDetails(
                txtCardNumber.Text,
                expiryMonth,
                expiryYear,
                txtCvv.Text,
                pinCode // Pass PIN to validation
            );

            if (isValid)
            {
                // Optional: Create Card object if needed later, now including PIN
                // Card card = new Card(txtCardNumber.Text, expiryMonth, expiryYear, txtCvv.Text, pinCode);
                MessageBox.Show(languageStrings[currentLanguage]["cardDetailsSuccess"]);
                Logger.Log("Card details submitted successfully."); // Log success
                // Proceed with next steps
            }
            else
            {
                MessageBox.Show(languageStrings[currentLanguage]["invalidCardDetails"]);
                Logger.Log("Card details submission failed: Validation error."); // Log validation failure
            }
        }

        private void HandleMainSubmit(object? sender, EventArgs e)
        {
            var cboLanguage = Controls["cboLanguage"] as ComboBox;
            string currentLanguage = (cboLanguage?.SelectedItem as string) ?? "English";

            var txtPhone = Controls["txtPhone"] as TextBox;
            var cboOperator = Controls["cboOperator"] as ComboBox;
            var txtCardNumber = Controls["txtCardNumber"] as TextBox;
            var txtDepositAmount = Controls["txtDepositAmount"] as TextBox;
            var txtExpiryMonth = Controls["txtExpiryMonth"] as TextBox;
            var txtExpiryYear = Controls["txtExpiryYear"] as TextBox;
            var txtCvv = Controls["txtCvv"] as TextBox;
            var txtPinCode = Controls["txtPinCode"] as TextBox; // Find PIN TextBox
            var nudCharityPercent = Controls["nudCharityPercent"] as NumericUpDown; // Find charity NumericUpDown

            bool isPhoneValid = txtPhone != null && IsValidPhoneNumber(txtPhone.Text);

            if (!isPhoneValid)
            {
                MessageBox.Show(
                    languageStrings[currentLanguage]["phoneEmptyError"],
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log(
                    $"Main submit validation failed: Phone number empty or invalid. Input: {txtPhone?.Text}"
                );
                return;
            }

            bool isOperatorSelected = cboOperator == null || cboOperator.SelectedItem == null;

            if (isOperatorSelected)
            {
                MessageBox.Show(
                    languageStrings[currentLanguage]["operatorNotSelectedError"],
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log("Main submit validation failed: Operator not selected.");
                return;
            }
            bool isCardNumberEmpty =
                txtCardNumber == null || string.IsNullOrWhiteSpace(txtCardNumber.Text);

            if (isCardNumberEmpty)
            {
                MessageBox.Show(
                    languageStrings[currentLanguage]["cardNumberEmptyError"],
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log("Main submit validation failed: Card number empty.");
                return;
            }
            bool isDepositAmountValid =
                txtDepositAmount != null
                && decimal.TryParse(
                    txtDepositAmount.Text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal amount
                )
                && amount > 0;

            if (!isDepositAmountValid)
            {
                MessageBox.Show(
                    languageStrings[currentLanguage]["amountInvalidError"],
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log(
                    $"Main submit validation failed: Invalid deposit amount. Input: {txtDepositAmount?.Text}"
                );
                return;
            }

            // Ensure charity percentage control exists
            if (nudCharityPercent == null)
            {
                MessageBox.Show(
                    "Internal Error: Charity percentage control not found.",
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log(
                    "Main submit failed: Charity percentage control (nudCharityPercent) not found."
                );
                return;
            }

            // Validate charity percentage (redundant with NumericUpDown limits, but good practice)
            decimal charityPercent = nudCharityPercent.Value;
            if (charityPercent < 0 || charityPercent > 100)
            {
                MessageBox.Show(
                    "Invalid charity percentage. Please enter a value between 0 and 100.", // Consider localizing this
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log(
                    $"Main submit validation failed: Invalid charity percentage. Input: {charityPercent}"
                );
                return;
            }

            // Gather data
            string phoneNumber = txtPhone.Text;
            string selectedOperator = cboOperator.SelectedItem.ToString()!;
            decimal depositAmount = decimal.Parse(
                txtDepositAmount.Text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
            string cardNumber = txtCardNumber.Text;

            // Declare variables before the if statement
            int expiryMonth = 0;
            int expiryYear = 0;

            bool isCardDetailsInvalid =
                txtExpiryMonth == null
                || txtExpiryYear == null
                || txtCvv == null
                || txtPinCode == null // Check if PIN TextBox exists
                || !int.TryParse(txtExpiryMonth.Text, out expiryMonth) // Use pre-declared variable
                || !int.TryParse(txtExpiryYear.Text, out expiryYear) // Use pre-declared variable
                // Basic PIN validation
                || string.IsNullOrWhiteSpace(txtPinCode.Text)
                || !txtPinCode.Text.All(char.IsDigit);

            if (isCardDetailsInvalid)
            {
                MessageBox.Show(
                    languageStrings[currentLanguage]["invalidCardDetails"],
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log("Main submit validation failed: Invalid card expiry format or PIN.");
                return;
            }

            string pinCode = txtPinCode.Text; // Get PIN

            // Assuming Bank.FindUserCard now needs the PIN code
            Card? foundCard = Bank.FindUserCard(
                banks,
                cardNumber,
                expiryMonth,
                expiryYear,
                txtCvv.Text,
                pinCode // Pass PIN code
            );

            double fee = Bank.GetFee(banks, cardNumber);
            Console.WriteLine($"Fee for the bank is {fee}");

            // Display fee warning message box
            string feeWarningTitle = languageStrings[currentLanguage]["feeWarningTitle"];
            string feeWarningMessageTemplate = languageStrings[currentLanguage][
                "feeWarningMessage"
            ];

            string feeWarningMessage = string.Format(
                feeWarningMessageTemplate,
                (fee * 100).ToString("F2")
            );

            MessageBox.Show(
                feeWarningMessage,
                feeWarningTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            Logger.Log($"Displayed fee warning: {feeWarningMessage}"); // Log the warning display

            if (foundCard == null)
            {
                MessageBox.Show(
                    languageStrings[currentLanguage]["invalidCardDetails"],
                    languageStrings[currentLanguage]["mainSubmitErrorTitle"],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.Log("Main submit validation failed: Card not found or details invalid.");
                return;
            }

            // Mask card number (all but last 4 digits)
            string maskedCardNumber;

            if (cardNumber.Length >= 4)
            {
                maskedCardNumber =
                    new string('*', cardNumber.Length - 4)
                    + cardNumber.Substring(cardNumber.Length - 4);
            }
            else // If card number is less than 4 digits, mask all of it.
            {
                maskedCardNumber = new string('*', cardNumber.Length);
            }

            // Create Receipt object
            Receipt receipt = new Receipt
            {
                PhoneNumber = phoneNumber,
                Operator = selectedOperator,
                Amount = depositAmount,
                Date = DateTime.Now, // Use current date and time for the receipt
                MaskedCardNumber = maskedCardNumber,
            };

            // Find the checkbox
            var chkPrintReceipt = Controls["chkPrintReceipt"] as CheckBox;

            if (chkPrintReceipt != null && chkPrintReceipt.Checked)
            {
                try
                {
                    string savedFileName = receipt.ToXmlAndSave();
                    Logger.Log(
                        $"Receipt saved successfully for phone {phoneNumber}, operator {selectedOperator}, amount {depositAmount}, charity {charityPercent}%, masked card {maskedCardNumber}. File: {savedFileName}"
                    );
                    MessageBox.Show(
                        string.Format(
                            languageStrings[currentLanguage]["receiptSavedSuccessMessage"],
                            savedFileName
                        ),
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error saving receipt: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    Logger.Log(
                        $"Error saving receipt for phone {phoneNumber}, operator {selectedOperator}, amount {depositAmount}, charity {charityPercent}%, masked card {maskedCardNumber}. Error: {ex.Message}"
                    );
                }
            }
            else
            {
                // Log that receipt was generated but not saved, or simply do nothing further
                Logger.Log(
                    $"Receipt generated but not saved as per user preference. Phone: {phoneNumber}, Operator: {selectedOperator}, Amount: {depositAmount}, Charity: {charityPercent}%, Masked Card: {maskedCardNumber}"
                );
                MessageBox.Show(
                    "Payment processed, receipt not saved.", // Consider localizing this message too
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void UpdateUILanguage(string language)
        {
            if (!languageStrings.ContainsKey(language))
                return;

            // Update all controls with the appropriate language strings
            foreach (Control control in this.Controls)
            {
                switch (control.Name)
                {
                    case "lblWelcome":
                        control.Text = languageStrings[language]["lblWelcome"];
                        break;
                    case "lblPhone":
                        control.Text = languageStrings[language]["phoneLabel"];
                        break;
                    case "btnOperatorSubmit":
                        // Ensure "btnSubmit" exists for the language before concatenating
                        string operatorSubmitText = languageStrings[language]
                            .TryGetValue("btnSubmit", out var submitText)
                            ? submitText
                            : "Submit";
                        control.Text = operatorSubmitText + " Operator";
                        break;
                    case "lblCardNumber":
                        control.Text = languageStrings[language]["lblCardNumber"];
                        break;
                    case "lblExpiryMonth":
                        control.Text = languageStrings[language]["lblExpiryMonth"];
                        break;
                    case "lblExpiryYear":
                        control.Text = languageStrings[language]["lblExpiryYear"];
                        break;
                    case "lblCvv":
                        control.Text = languageStrings[language]["lblCvv"];
                        break;
                    case "btnCardSubmit":
                        string cardSubmitText = languageStrings[language]
                            .TryGetValue("btnSubmit", out var csText)
                            ? csText
                            : "Submit";
                        control.Text = cardSubmitText + " Card Details";
                        break;
                    case "lblPinCode":
                        control.Text = languageStrings[language]["lblPinCode"];
                        break;
                    case "lblDepositAmount":
                        control.Text = languageStrings[language]["lblDepositAmount"];
                        break;
                    case "btnMainSubmit": // Added case for the new button
                        control.Text = languageStrings[language]["btnMainSubmitText"];
                        break;
                    case "chkPrintReceipt": // Added case for the checkbox
                        control.Text = languageStrings[language]["chkPrintReceiptText"];
                        break;
                    case "lblCharityPercent": // Added case for the charity label
                        control.Text = languageStrings[language]["lblCharityPercent"];
                        break;
                }
            }
        }
    }
}
