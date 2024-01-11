using walletz.MessageObjects;

namespace walletz.Interfaces;

public interface IUserAction
{
    public UserResponse CreateUser(UserRequest newUser);

    public bool VerifyUser(string phone, string key);


}
