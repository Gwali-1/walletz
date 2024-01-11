namespace walletz.Models;


public class User
{

    public int Id { get; set; }

    public String PhoneNumber { get; set; } = string.Empty;

    public String Key { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public int ExistingWalletNumber {get; set;}

    //wallets
    public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>{};



}
