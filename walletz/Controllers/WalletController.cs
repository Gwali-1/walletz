namespace walletz.Controllers;


[ApiController]
[Route("api/[controller]")]
public class VeteranController : ControllerBase
{

    private readonly ILogger _logger;
    private readonly IMember _member;
    private readonly DbSession _repo;
    public VeteranController(ILogger<MembersController> logger, IMember member, DbSession repo)
    {
        _logger = logger;
        _member = member;
        _repo = repo;
    }
}

