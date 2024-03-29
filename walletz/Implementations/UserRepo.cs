using walletz.Interfaces;
using walletz.DTOs;
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

    public bool DecreaseUserWalletNumber(string phoneNumber)
    {
        User? user = _datacontext.users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
        if (user == null)
        {
            return false;
        }

        user.ExistingWalletNumber -= 1;
        _datacontext.SaveChanges();
        return true;

    }

    public bool UserExist(string phoneNumber)
    {
        return _datacontext.users.Any(u => u.PhoneNumber == phoneNumber);
    }

}
