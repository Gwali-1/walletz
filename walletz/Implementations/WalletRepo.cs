using walletz.Interfaces;
using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Implementations;



public class WalletRepo : IWalletAction
{
    private readonly ILogger _logger;
    private readonly SessionContext _datacontext;

    public WalletRepo(ILogger<WalletRepo> logger, SessionContext datacontext)
    {
        _logger = logger;
        _datacontext = datacontext;

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




    public bool DeleteWallet(string walletId)
    {
        try
        {

            Wallet? walletItem = _datacontext.wallets.Where(w => w.Id == walletId).FirstOrDefault();
            if (walletItem == null)
            {
                return false;
            }
            _datacontext.Remove(walletItem);
            return true;


        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;

        }
    }

    public ICollection<Wallet> GetWallets(string owner)
    {
        List<Wallet> walletitems = _datacontext.wallets.Where(w => w.Owner == owner).ToList();
        return walletitems;
    }
}
