using System.ComponentModel.DataAnnotations;

namespace walletz.Models;


public class Wallet
{

    public String Id { get; set; } = string.Empty;

    public String Name { get; set; } = string.Empty;

    public String Type { get; set; } = string.Empty;

    public String AccountNumber { get; set; } = string.Empty;

    public String AccountScheme { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public String Owner { get; set; } = string.Empty;

    //wallets
    [Required]
    public User user { get; set; }



}
