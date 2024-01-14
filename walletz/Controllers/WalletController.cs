using Microsoft.AspNetCore.Mvc;
using walletz.DTOs;
using walletz.Interfaces;
using walletz.Models;

namespace walletz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{

    private readonly ILogger _logger;
    private readonly IWalletAction _walletDb;
    private readonly IUserAction _userDb;
    private readonly IVerify _verifier;
    public WalletController(ILogger<UserController> logger, IWalletAction db, IVerify verifier, IUserAction userDb)
    {
        _logger = logger;
        _walletDb = db;
        _verifier = verifier;
        _userDb = userDb;
    }


    private string TransformPhone(string phone)
    {
        if (phone.StartsWith("233") && phone.Length == 12)
        {
            return phone;
        }
        string formatted = "233" + phone.Substring(1, phone.Length - 1);
        return formatted;
    }


    [HttpPost("create")]
    public IActionResult CreateWallet(WalletRequest newWallet, [FromQuery] string phone, [FromQuery] string key)
    {
        //make sure wallet number is not more than 5;

        phone = TransformPhone(phone);

        bool validUser = _verifier.VerifyUser(phone, key);
        if (!validUser)
        {
            return BadRequest("Invalid auth details");
        }

        bool validWalletNumber = _verifier.ValidWalletLimit(phone);

        if (!validWalletNumber)
        {
            return BadRequest("Wallet limit of 5 reached");
        }


        string result = _verifier.VeryAccountType(newWallet);

        if (result != string.Empty)
        {
            return BadRequest(result);
        }

        //if its momo
        if (newWallet.Type.Trim().ToUpper() == "MOMO")
        {
            bool validNumber = _verifier.VerifyPhoneNumber(newWallet.AccountNumber);

            if (!validNumber)
            {
                return BadRequest("Invalid number for account type");
            }


            newWallet.AccountNumber = TransformPhone(newWallet.AccountNumber.Trim());

        }


        bool walletExist = _walletDb.WalletExits(newWallet.AccountNumber.Trim(), newWallet.Name.Trim(), phone);
        if (walletExist)
        {
            return BadRequest("Wallet already exists");
        }


        User user = _userDb.GetUser(phone.Trim());

        if (newWallet.Type.Trim().ToUpper() == "CARD")
        {
            newWallet.AccountNumber = newWallet.AccountNumber.Trim().Substring(0, 6) + new string('*', 16);

        }

        bool walletCreated = _walletDb.CreateWallet(newWallet, user);
        if (!walletCreated)
        {
            return StatusCode(500, "Could not add wallet");
        }

        _userDb.IncreaseUserWalletNumber(phone);


        return Ok("wallet added");

    }


    [HttpGet("all")]
    public IActionResult All([FromQuery] string phone, [FromQuery] string key)
    {
        phone = phone.Trim();
        key = key.Trim();

        bool user = _verifier.VerifyUser(TransformPhone(phone), key);
        if (!user)
        {
            return BadRequest("Invalid auth details");
        }

        ICollection<WalletResponse> userWallets = _walletDb.GetWallets(TransformPhone(phone));
        return Ok(userWallets);



    }


    [HttpGet("{walletId}")]
    public IActionResult GetWallet([FromQuery] string phone, [FromQuery] string key, string walletId)
    {
        phone = phone.Trim();
        key = key.Trim();


        bool user = _verifier.VerifyUser(TransformPhone(phone), key);
        if (!user)
        {
            return BadRequest("Invalid auth details");
        }

        WalletResponse walletItem = _walletDb.GetWallet(walletId);
        if (walletItem == null)
        {
            return NotFound("wallet does not exist");
        }


        return Ok(walletItem);

    }


    [HttpDelete("{walletId}")]
    public IActionResult DeleteWallet([FromQuery] string phone, [FromQuery] string key, string walletId)
    {
        phone = phone.Trim();
        key = key.Trim();

        string format233 = TransformPhone(phone);


        bool user = _verifier.VerifyUser(format233, key);
        if (!user)
        {
            return BadRequest("Invalid auth details");
        }

        Wallet walletDeleted = _walletDb.DeleteWallet(walletId);
        if (walletDeleted == null)
        {
            return BadRequest("Wallet does not exist");
        }

        //decrease wallet number 
        _userDb.DecreaseUserWalletNumber(format233);

        return Ok($"Wallet with id {walletDeleted.Id} Removed");


    }

}

