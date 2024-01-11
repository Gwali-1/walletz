
using Microsoft.AspNetCore.Mvc;
using walletz.Implementations;
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


    [HttpPost("create")]
    public IActionResult CreateUser(UserRequest newUser)
    {

        bool validPhoneNumber = _verifier.verifyPhoneNumber(newUser.PhoneNumber);
        if (!validPhoneNumber)
        {
            return BadRequest("Invalid Phone number");
        }


        UserResponse createdUser = _db.CreateUser(newUser);
        if (createdUser == null)
        {
            return StatusCode(500, "New user could not be created");
        }

        return Ok(createdUser);


    }


}

