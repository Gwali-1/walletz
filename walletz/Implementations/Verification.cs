using System.Security.Cryptography;
using System.Text;
using walletz.DTOs;
using walletz.Interfaces;
using walletz.Models;

namespace walletz.Implementations;



public class Verification : IVerify
{
    private readonly ILogger _logger;
    private readonly SessionContext _datacontext;
    private List<string> allowedTypes = new List<string> { "MOMO", "CARD" };
    private List<string> allowedMomoSchemes = new List<string> { "MTN", "VODAFONE", "AIRTELTIGO" };
    private List<string> allowedCardSchemes = new List<string> { "VISA", "MASTERCARD" };

    public Verification(ILogger<WalletRepo> logger, SessionContext datacontext)
    {
        _logger = logger;
        _datacontext = datacontext;

    }

    public string GenerateUniqueid(string accountNumber)
    {
        using SHA256 crypt = SHA256.Create();

        byte[] uniqueBytes = Encoding.UTF8.GetBytes(accountNumber);
        byte[] hashBytes = crypt.ComputeHash(uniqueBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "");

    }

    public string GenerateUniquekey()  // return only uuid without hashing?
    {

        Guid uniqueId = Guid.NewGuid();

        return uniqueId.ToString("N");
        /* using SHA256 crypt = SHA256.Create(); */
        /**/
        /* byte[] uniqueBytes = uniqueId.ToByteArray(); */
        /* byte[] hashBytes = crypt.ComputeHash(uniqueBytes); */
        /* return BitConverter.ToString(hashBytes).Replace("-", ""); */
        /**/
    }


    public bool VerifyPhoneNumber(string phoneNumber)
    {

        return (phoneNumber.Length == 10 && phoneNumber.StartsWith("0")) || (phoneNumber.Length == 12 && phoneNumber.StartsWith("233"));

        /* return (phoneNumber.Length == 10 | phoneNumber.Length = 12) && (phoneNumber.StartsWith("0") | phoneNumber.StartsWith("233")); */
    }

    public string VeryAccountType(WalletRequest newWallet)
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

            if (newWallet.AccountNumber.Length < 16)
            {

                return "Card number can't be less than 16";
            }

        }

        return string.Empty;

    }


    public bool ValidWalletLimit(string phoneNumber)
    {
        User? user = _datacontext.users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
        if (user == null)
        {
            return false;
        }

        return user.ExistingWalletNumber < 5;
    }

    public bool VerifyUser(string phone, string key)
    {
        try
        {
            User? user = _datacontext.users.Where(u => u.PhoneNumber == phone).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            if (user.Key != key.Trim())
            {
                return false;
            }

            return true;

        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return false;
        }
    }


}
