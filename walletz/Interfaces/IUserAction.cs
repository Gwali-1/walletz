using walletz.DTOs;
using walletz.Models;

namespace walletz.Interfaces;

public interface IUserAction
{
    public UserResponse CreateUser(UserRequest newUser);

    public bool VerifyUser(string phone, string key);

    public bool userExist(string phoneNumber);

    public User GetUser(string phonneNumber);

    public bool IncreaseUserWalletNumber(string phoneNumber);

    public bool DecreaseUserWalletNumber(string phoneNumber);

    public bool validWalletLimit(string phoneNumber);


}
