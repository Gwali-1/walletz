using walletz.Interfaces;
using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Implementations;



public class Verification : IVerify
{
    private readonly ILogger _logger;

    private List<string> allowedTypes = new List<string> { "MOMO", "CARD" };
    private List<string> allowedMomoSchemes = new List<string> { "MTN", "VODAFONE", "AIRTELTIGO" };
    private List<string> allowedCardSchemes = new List<string> { "VISA", "MASTERCARD" };

    public Verification(ILogger<WalletRepo> logger)
    {
        _logger = logger;

    }

    public bool verifyPhoneNumber(string phoneNumber)
    {

        return phoneNumber.Length == 10 && phoneNumber.StartsWith("0");
    }

    public string veryAccountType(WalletRequest newWallet)
    {
        String InputWalletType = newWallet.Type.Trim().ToUpper();
        String InputSchemaType = newWallet.AccountScheme.Trim().ToUpper();

        if (!allowedTypes.Contains(InputWalletType))
        {
            return "Invalid Wallet Type | momo or card only allowd";
        }


        if (InputWalletType == "MOMO")
        {
            if (!allowedMomoSchemes.Contains(InputSchemaType))
            {
                return "Invalid acccount scheme for chosen wallet type | allowed are mtn, vodafone or airteltigo";
            }

        }


        if (InputWalletType == "CARD")
        {
            if (!allowedMomoSchemes.Contains(InputSchemaType))
            {
                return "Invalid acccount scheme for chosen wallet type | allowed are visa or  mastercard";
            }

        }

        return string.Empty;

    }
}
