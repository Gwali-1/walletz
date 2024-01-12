using walletz.DTOs;

namespace walletz.Interfaces;

public interface IVerify
{
    public string veryAccountType(WalletRequest newwallet);

    public bool verifyPhoneNumber(string phoneNumber);

    public string GenerateUniqueid(string accountNumber);

    public string GenerateUniquekey();


}
