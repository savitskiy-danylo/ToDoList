using System.Diagnostics;
using api.DAL;
using api.Data;
using api.DTO;
using api.Extensions;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly UnitOfWork _uow;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(
      UnitOfWork uow,
      UserManager<AppUser> userManager,
      ILogger<AccountController> logger,
      TokenService tokenService,
      IMapper mapper
      )
    {
      _logger = logger;
      _tokenService = tokenService;
      _mapper = mapper;
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

      var result = await _userManager.CreateAppUser(_uow.ListRepository, user, registerDto.Password);

      if (!result.Succeeded)
      {
        string errors = "";
        result.Errors.Select(error => errors += error.Description + "\n");
        _logger.LogError("Error creating new user." + errors);
        return BadRequest("Error creating new user.");
      }

      var dto = _mapper.Map<AppUser, UserDto>(user);
      dto.Token = _tokenService.CreateToken(user);
      return dto;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(RegisterDto loginDto)
    {
      var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

      if (user == null) return Unauthorized("Invalid username");
      if (string.IsNullOrWhiteSpace(loginDto.Password)
        && await _userManager.CheckPasswordAsync(user, loginDto.Password) == false)
        return Unauthorized("Invalid password");
      var dto = _mapper.Map<AppUser, UserDto>(user);
      dto.Token = _tokenService.CreateToken(user);
      return dto;
    }
  }
}