using walletz.DTOs;
using walletz.Models;

namespace walletz.Interfaces;

public interface IWalletAction
{
    public bool CreateWallet(WalletRequest newWallet, User owner);

    public ICollection<WalletResponse> GetWallets(string owner);

    public WalletResponse GetWallet(string walletId);

    public Wallet DeleteWallet(string walletId);

    public bool WalletExits(string accountNumber, string name);


}
