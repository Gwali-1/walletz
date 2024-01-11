using Microsoft.AspNetCore.Mvc;
using walletz.MessageObjects;
using walletz.Interfaces;

namespace walletz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger _logger;
    private readonly IUserAction _db;
    private readonly IVerify _verifier;
    public UserController(ILogger<UserController> logger, IUserAction db, IVerify verifier)
    {
        _logger = logger;
        _db = db;
        _verifier = verifier;
    }


    private string transformPhone(string phone)
    {
        string formatted = "233" + phone.Substring(1, phone.Length - 1);
        _logger.LogInformation($"This is the formatted to 2333 --> {formatted}");
        return formatted;
    }


    [HttpPost("create")]
    public IActionResult CreateUser(UserRequest newUser)
    {
        //transform number to 233 fromat

        bool validPhoneNumber = _verifier.verifyPhoneNumber(newUser.PhoneNumber);
        if (!validPhoneNumber)
        {
            return BadRequest("Invalid Phone number");
        }

        string format233 = transformPhone(newUser.PhoneNumber);

        bool userExist = _db.userExist(format233);

        if (userExist)
        {
            return BadRequest("User with phone number exist");
        }


        newUser.PhoneNumber = format233;
        UserResponse createdUser = _db.CreateUser(newUser);
        if (createdUser == null)
        {
            return StatusCode(500, "New user could not be created");
        }

        return Ok(createdUser);


    }


}

