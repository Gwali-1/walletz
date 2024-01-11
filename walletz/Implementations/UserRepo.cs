using walletz.Interfaces;
using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Implementations;



public class UserRepo : IUserAction
{
    private readonly ILogger _logger;
    private readonly SessionContext _datacontext;

    public WalletRepo(ILogger<WalletRepo> logger, SessionContext datacontext)
    {
        _logger = logger;
        _datacontext = datacontext;

    }

    public UserResponse CreateUser(UserRequest newUser)
    {

        try
        {

            User UsertItem = new User
            {
                PhoneNumber = newUser.PhoneNumber,
                Key = "unique key here",

            };

            _datacontext.users.Add(walletItem);
            _datacontext.SaveChanges();

            return new UserResponse{}
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    public bool CreateWallet(WalletRequest newWallet, User owner)
    {
        try
        {

            Wallet walletItem = new Wallet
            {
                Id = "hash here",
                Name = newWallet.Name,
                AccountNumber = newWallet.AccountNumber,
                AccountScheme = newWallet.AccountScheme,
                Owner = owner.PhoneNumber,
                user = owner

            };

            _datacontext.wallets.Add(walletItem);
            _datacontext.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool VerifyUser(string phone, string key)
    {
        throw new NotImplementedException();
    }
}
