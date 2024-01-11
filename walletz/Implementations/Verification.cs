using System.Security.Cryptography;
using System.Text;
using walletz.Interfaces;
using walletz.MessageObjects;

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

    public string GenerateUniqueid(string accountNumber)
    {
        using SHA256 crypt = SHA256.Create();

        byte[] uniqueBytes = Encoding.UTF8.GetBytes(accountNumber);
        byte[] hashBytes = crypt.ComputeHash(uniqueBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 7);

    }

    public string GenerateUniquekey()
    {

        Guid uniqueId = Guid.NewGuid();
        using SHA256 crypt = SHA256.Create();

        byte[] uniqueBytes = uniqueId.ToByteArray();
        byte[] hashBytes = crypt.ComputeHash(uniqueBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "");

    }


    public bool verifyPhoneNumber(string phoneNumber)
    {

        return (phoneNumber.Length == 10 && phoneNumber.StartsWith("0")) | (phoneNumber.Length == 12 && phoneNumber.StartsWith("233"));

        /* return (phoneNumber.Length == 10 | phoneNumber.Length = 12) && (phoneNumber.StartsWith("0") | phoneNumber.StartsWith("233")); */
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
            if (!allowedCardSchemes.Contains(InputSchemaType))
            {
                return "Invalid acccount scheme for chosen wallet type | allowed are visa or  mastercard";
            }

        }

        return string.Empty;

    }

}
