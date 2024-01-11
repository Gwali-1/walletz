using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Interfaces;

public interface IWalletAction
{
    public bool CreateWallet(WalletRequest newWallet, User owner);

    public ICollection<WalletResponse> GetWallets(string owner);

    public WalletResponse GetWallet(string walletId);

    public bool DeleteWallet(string walletId);

    public bool WalletExits(string accountNumber);


}
