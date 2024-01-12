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
    private readonly IWalletAction _walletdb;
    private readonly IUserAction _userDb;
    private readonly IVerify _verifier;
    public WalletController(ILogger<UserController> logger, IWalletAction db, IVerify verifier, IUserAction userDb)
    {
        _logger = logger;
        _walletdb = db;
        _verifier = verifier;
        _userDb = userDb;
    }


    private string transformPhone(string phone)
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

        phone = transformPhone(phone);

        bool validUser = _userDb.VerifyUser(phone, key);
        if (!validUser)
        {
            return BadRequest("Invalid auth details");
        }

        bool validWalletNumber = _userDb.validWalletLimit(phone);

        if (!validWalletNumber)
        {
            return BadRequest("Wallet limit of 5 reached");
        }


        string result = _verifier.veryAccountType(newWallet);

        if (result != string.Empty)
        {
            return BadRequest(result);
        }

        //if its momo
        if (newWallet.Type.Trim().ToUpper() == "MOMO")
        {
            bool validNumber = _verifier.verifyPhoneNumber(newWallet.AccountNumber);

            if (!validNumber)
            {
                return BadRequest("Invalid number for account type");
            }


            newWallet.AccountNumber = transformPhone(newWallet.AccountNumber.Trim());

        }


        bool walletExist = _walletdb.WalletExits(newWallet.AccountNumber.Trim(), newWallet.Name.Trim());
        if (walletExist)
        {
            return BadRequest("Wallet already exists");
        }


        User user = _userDb.GetUser(phone.Trim());

        if (newWallet.Type.Trim().ToUpper() == "CARD")
        {
            newWallet.AccountNumber = newWallet.AccountNumber.Trim().Substring(0, 6) + new string('*', 16);

        }





        bool walletCreated = _walletdb.CreateWallet(newWallet, user);
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

        bool user = _userDb.VerifyUser(transformPhone(phone), key);
        if (!user)
        {
            return BadRequest("Invalid auth details");
        }

        ICollection<WalletResponse> userWallets = _walletdb.GetWallets(transformPhone(phone));
        return Ok(userWallets);



    }


    [HttpGet("{walletId}")]
    public IActionResult GetWallet([FromQuery] string phone, [FromQuery] string key, string walletId)
    {
        phone = phone.Trim();
        key = key.Trim();


        bool user = _userDb.VerifyUser(transformPhone(phone), key);
        if (!user)
        {
            return BadRequest("Invalid auth details");
        }

        WalletResponse walletItem = _walletdb.GetWallet(walletId);
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


        bool user = _userDb.VerifyUser(transformPhone(phone), key);
        if (!user)
        {
            return BadRequest("Invalid auth details");
        }

        Wallet walletDeleted = _walletdb.DeleteWallet(walletId);
        if (walletDeleted == null)
        {
            return BadRequest("Wallet does not exist");
        }

        //decrease wallet number 
        _userDb.DecreaseUserWalletNumber(transformPhone(phone));

        return Ok($"Wallet with id {walletDeleted.Id} Removed");







    }







}

