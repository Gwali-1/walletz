using walletz.DTOs;

namespace walletz.Interfaces;

public interface IVerify
{
    public string VeryAccountType(WalletRequest newwallet);

    public bool VerifyPhoneNumber(string phoneNumber);

    public string GenerateUniqueid(string accountNumber);

    public string GenerateUniquekey();

    public bool ValidWalletLimit(string phoneNumber);

    public bool VerifyUser(string phone, string key);


}
