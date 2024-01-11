using walletz.Interfaces;
using walletz.MessageObjects;
using walletz.Models;

namespace walletz.Implementations;



public class UserRepo : IUserAction
{
    private readonly ILogger _logger;
    private readonly SessionContext _datacontext;
    private readonly IVerify _utilty;

    public UserRepo(ILogger<WalletRepo> logger, SessionContext datacontext, IVerify utility)
    {
        _logger = logger;
        _datacontext = datacontext;
        _utilty = utility;

    }



    public UserResponse CreateUser(UserRequest newUser)
    {

        try
        {
            string key = _utilty.GenerateUniquekey();

            User userItem = new User
            {
                PhoneNumber = newUser.PhoneNumber.Trim(),
                Key = key,

            };

            _datacontext.users.Add(userItem);
            _datacontext.SaveChanges();

            return new UserResponse { PhoneNumber = newUser.PhoneNumber, Key = key };
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return null;
        }

    }

    public User GetUser(string phoneNumber)
    {
        User? user = _datacontext.users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
        return user;

    }

    public bool IncreaseUserWalletNumber(string phoneNumber)
    {
        User? user = _datacontext.users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
        if (user == null)
        {
            return false;
        }

        user.ExistingWalletNumber += 1;
        _datacontext.SaveChanges();
        return true;

    }

    public bool userExist(string phoneNumber)
    {
        return _datacontext.users.Any(u => u.PhoneNumber == phoneNumber);
    }

    public bool validWalletLimit(string phoneNumber)
    {
        User? user = _datacontext.users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
        if (user == null)
        {
            return false;
        }

        return user.ExistingWalletNumber < 5;
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
