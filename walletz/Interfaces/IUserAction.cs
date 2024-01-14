using walletz.DTOs;
using walletz.Models;

namespace walletz.Interfaces;

public interface IUserAction
{
    public UserResponse CreateUser(UserRequest newUser);

    public bool UserExist(string phoneNumber);

    public User GetUser(string phonneNumber);

    public bool IncreaseUserWalletNumber(string phoneNumber);

    public bool DecreaseUserWalletNumber(string phoneNumber);

}
