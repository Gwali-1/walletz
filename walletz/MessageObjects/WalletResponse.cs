namespace walletz.MessageObjects;

public class WalletResponse
{

    public String Id { get; set; } = string.Empty;

    public String Name { get; set; } = string.Empty;

    public String AccountNumber { get; set; } = string.Empty;

    public String AccountScheme { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public String Owner { get; set; } = string.Empty;

}
