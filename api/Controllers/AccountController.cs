using api.DAL;
using api.Data;
using api.DTO;
using api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly UnitOfWork _uow;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly TokenService _tokenService;

    public AccountController(
      UnitOfWork uow,
      UserManager<AppUser> userManager,
      ILogger<AccountController> logger,
      TokenService tokenService)
    {
      _logger = logger;
      _tokenService = tokenService;
      _userManager = userManager;
      _uow = uow;

    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      AppUser user = new AppUser
      {
        UserName = registerDto.UserName
      };

      var result = await _userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded)
      {
        string errors = "";
        result.Errors.Select(error => errors += error.Description + "\n");
        _logger.LogError("Error creating new user." + errors);
        return BadRequest("Error creating new user.");
      }

      return new UserDto
      {
        UserName = user.UserName,
        Token = _tokenService.CreateToken(user)
      };
    }
  }
}