using walletz.DTOs;
using walletz.Interfaces;
using walletz.Models;

namespace walletz.Implementations;



public class WalletRepo : IWalletAction
{
    private readonly ILogger _logger;
    private readonly SessionContext _datacontext;
    private readonly IVerify _utility;



    public WalletRepo(ILogger<WalletRepo> logger, SessionContext datacontext, IVerify utility)
    {
        _logger = logger;
        _datacontext = datacontext;
        _utility = utility;

    }



    public bool CreateWallet(WalletRequest newWallet, User owner)
    {
        try
        {

            Wallet walletItem = new Wallet
            {
                Id = _utility.GenerateUniqueid(newWallet.AccountNumber),
                Type = newWallet.Type.Trim(),
                Name = newWallet.Name.Trim(),
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
            _logger.LogInformation(e.Message);
            return false;
        }
    }




    public Wallet DeleteWallet(string walletId)
    {
        try
        {

            Wallet? walletItem = _datacontext.wallets.Where(w => w.Id == walletId).FirstOrDefault();
            if (walletItem == null)
            {
                return null;
            }
            _datacontext.Remove(walletItem);
            _datacontext.SaveChanges();
            return walletItem;


        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return null;

        }
    }

    public WalletResponse GetWallet(string walletId)
    {
        try
        {

            WalletResponse? walletItem = _datacontext.wallets.Where(w => w.Id == walletId).Select(w => new WalletResponse
            {
                Id = w.Id,
                Name = w.Name,
                Type = w.Type,
                AccountNumber = w.AccountNumber,
                AccountScheme = w.AccountScheme,
                CreatedAt = w.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Owner = w.Owner

            }).FirstOrDefault();

            return walletItem;


        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return null;

        }

    }

    public ICollection<WalletResponse> GetWallets(string owner)
    {
        List<WalletResponse> walletitems = _datacontext.wallets.Where(w => w.Owner == owner).Select(w => new WalletResponse
        {
            Id = w.Id,
            Name = w.Name,
            Type = w.Type,
            AccountNumber = w.AccountNumber,
            AccountScheme = w.AccountScheme,
            CreatedAt = w.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Owner = w.Owner


        }).ToList();
        return walletitems;
    }



    public bool WalletExits(string accountNumber, string name, string owner)
    {
        string walletId = _utility.GenerateUniqueid(accountNumber);
        bool exits = _datacontext.wallets.Any(w => w.Id == walletId && w.Name == name && w.Owner == owner);

        if (exits)
        {
            return true;
        }
        return false;
    }

}
