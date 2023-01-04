using api.DAL;
using api.Data;
using api.DTO;
using api.Extensions;
using api.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  [Authorize]
  public class ListController : BaseApiController
  {
    private readonly UnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ListController> _logger;

    public ListController(UnitOfWork uow, IMapper mapper, UserManager<AppUser> userManager, ILogger<ListController> logger)
    {
      _logger = logger;
      _userManager = userManager;
      _mapper = mapper;
      _uow = uow;

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ListDto>> GetList(string id)
    {
      var user = await _userManager.Users
        .Include(x => x.Lists)
        .FirstAsync(x => x.UserName == User.GetUsername());

      if (user.Lists.Where(x => x.Id == id) == null) return BadRequest("User don't have this list");

      var list = _uow.ListRepository.Get(x => x.Id == id, null, "Tasks").First();

      return _mapper.Map<ToDoList, ListDto>(list);

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListDto>>> GetLists()
    {
      var user = await _userManager.Users
        .Include(x => x.Lists)
        .ThenInclude(x => x.Tasks)
        .FirstAsync(x => x.UserName == User.GetUsername());

      List<ListDto> lists = new();
      foreach (var list in user.Lists)
      {
        lists.Add(_mapper.Map<ToDoList, ListDto>(list));
      }
      return lists;
    }


  }
}