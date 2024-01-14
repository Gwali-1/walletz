using Microsoft.AspNetCore.Mvc;
using walletz.DTOs;
using walletz.Interfaces;

namespace walletz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger _logger;
    private readonly IUserAction _userDb;
    private readonly IVerify _verifier;
    public UserController(ILogger<UserController> logger, IUserAction db, IVerify verifier)
    {
        _logger = logger;
        _userDb = db;
        _verifier = verifier;
    }


    private string TransformPhone(string phone)
    {
        string formatted = "233" + phone.Substring(1, phone.Length - 1);
        return formatted;
    }


    [HttpPost("create")]
    public IActionResult CreateUser(UserRequest newUser)
    {
        //transform number to 233 fromat

        bool validPhoneNumber = _verifier.VerifyPhoneNumber(newUser.PhoneNumber);
        if (!validPhoneNumber)
        {
            return BadRequest("Invalid Phone number");
        }

        string format233 = TransformPhone(newUser.PhoneNumber);

        bool userExist = _userDb.UserExist(format233);

        if (userExist)
        {
            return BadRequest("User with phone number exist");
        }


        newUser.PhoneNumber = format233;
        UserResponse createdUser = _userDb.CreateUser(newUser);
        if (createdUser == null)
        {
            return StatusCode(500, "New user could not be created");
        }

        return Ok(createdUser);


    }


}

