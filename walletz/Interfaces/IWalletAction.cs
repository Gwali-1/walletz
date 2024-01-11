using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Interfaces;

public interface IWalletAction
{
    public bool CreateWallet(WalletRequest newWallet, User owner);

    public ICollection<Wallet> GetWallets(string owner);

    public bool DeleteWallet(string walletId);


}
