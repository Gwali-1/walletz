using walletz.Interfaces;
using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Implementations;



public class UserRepo : IUserAction
{
    private readonly ILogger _logger;
    private readonly SessionContext _datacontext;

    public UserRepo(ILogger<WalletRepo> logger, SessionContext datacontext)
    {
        _logger = logger;
        _datacontext = datacontext;

    }

    public UserResponse CreateUser(UserRequest newUser)
    {

        try
        {

            User userItem = new User
            {
                PhoneNumber = newUser.PhoneNumber.Trim(),
                Key = "unique key here",

            };

            _datacontext.users.Add(userItem);
            _datacontext.SaveChanges();

            return new UserResponse { PhoneNumber = newUser.PhoneNumber, Key = "uniquew key" };
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return null;
        }

    }


    public bool VerifyUser(string phone, string key)
    {
        try
        {
            User? user = _datacontext.users.Where(u => u.PhoneNumber == phone).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            if (user.Key != key.Trim())
            {
                return false;
            }

            return true;

        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return false;
        }
        throw new NotImplementedException();
    }
}
