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

    [HttpPut("add-list")]
    public ActionResult<ListDto> AddList(ListDto newList)
    {
      if (newList.UserId != User.GetUserId()) return BadRequest("Wrond user id.");
      ToDoList list = _mapper.Map<ListDto, ToDoList>(newList);
      list.Id = Guid.NewGuid().ToString();
      list.Tasks = new List<ToDoTask>(){
        new(){
          Title = "Create new list",
          Description = "Create new list and new task",
          IsCompleted = true,
          CreatedAt = DateTime.UtcNow,
          ExpiryDate = DateTime.UtcNow,
          ListId = list.Id,
          Index = 1
        }
      };
      _uow.ListRepository.Insert(list);

      if (_uow.Save()) return _mapper.Map<ToDoList, ListDto>(list);

      return BadRequest("Cannot aply changes. [ADD LIST]");
    }

    [HttpDelete]
    public ActionResult DeleteList([FromQuery] DeleteParams deleteParams)
    {
      deleteParams.id = deleteParams.id.Replace('\'', ' ').Trim();
      var list = _uow.ListRepository.GetById(deleteParams.id);
      if (list == null) return BadRequest("List not found");
      if (list.UserId != User.GetUserId()) return BadRequest("User don't have sucha list");
      _uow.ListRepository.Delete(list);
      if (_uow.Save()) return NoContent();

      return BadRequest("Cannot apply cahnges. [DELETE LIST]");
    }

    [HttpPut]
    public ActionResult<ListDto> UpdateList(ListDto editedList)
    {
      var list = _uow.ListRepository.Get(x => x.Id == editedList.Id, null, "Tasks").First();
      if (list == null) return BadRequest("List not found");
      if (list.UserId != User.GetUserId()) return BadRequest("User don't have such a list.");


      _mapper.Map(editedList, list);
      _uow.ListRepository.Update(list);

      if (_uow.Save()) return _mapper.Map<ToDoList, ListDto>(list);

      return BadRequest("Cannot apply changes. [UPDATE LIST]");
    }
  }
}