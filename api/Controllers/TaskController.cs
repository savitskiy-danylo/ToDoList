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
  public class TaskController : BaseApiController
  {
    private readonly UnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ListController> _logger;

    public TaskController(UnitOfWork uow, IMapper mapper, UserManager<AppUser> userManager, ILogger<ListController> logger)
    {
      _logger = logger;
      _userManager = userManager;
      _mapper = mapper;
      _uow = uow;

    }

    [HttpPut]
    public ActionResult UpdateTask(TaskDto editedTask)
    {
      var task = _uow.TaskRepository.GetById(editedTask.Id);
      if (task == null) return BadRequest("Invalid request. Task does not exist");

      var list = _uow.ListRepository.GetById(task.ListId);
      if (list == null) return BadRequest("Invalid request. List does not exist");

      _mapper.Map(editedTask, task);
      _uow.TaskRepository.Update(task);
      if (_uow.Save()) return NoContent();

      return BadRequest("Error. No changes.");
    }

    [HttpDelete("delete-task/")]
    public async Task<ActionResult> DeleteTask([FromQuery] DeleteParams deleteParams)
    {
      deleteParams.id = deleteParams.id.Replace('\'', ' ').Trim();
      var task = _uow.TaskRepository.Get(x => x.Id.Equals(deleteParams.id), null, "List").FirstOrDefault();
      if (task == null) return BadRequest("Task does not exist.");
      var user = await _userManager.Users.Include(x => x.Lists).FirstAsync(x => x.UserName == User.GetUsername());
      var list = user.Lists.First(x => x.Id == task.List.Id);
      if (list == null) return BadRequest("User don't have such a list.");

      _uow.TaskRepository.Delete(task);

      if (_uow.Save()) return NoContent();

      return BadRequest("Cannot aply changes. [DELETE TASK]");
    }

    [HttpPut("add-task")]
    public async Task<ActionResult<TaskDto>> AddTask(TaskDto addedTask)
    {
      var list = _uow.ListRepository.GetById(addedTask.ListId);
      if (list == null) return BadRequest("List not found");
      if (list.UserId != User.GetUserId()) return BadRequest("User don't have such a list");
      ToDoTask task = _mapper.Map<TaskDto, ToDoTask>(addedTask);
      task.Id = Guid.NewGuid().ToString();
      _uow.TaskRepository.Insert(task);
      if (_uow.Save()) return _mapper.Map<ToDoTask, TaskDto>(task);

      return BadRequest("Cannot aply changes. [ADD TASK]");
    }
  }
}