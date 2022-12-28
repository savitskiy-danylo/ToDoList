using api.Data;
using api.DTO;
using AutoMapper;

namespace api.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<AppUser, UserDto>();
      CreateMap<ToDoList, ListDto>();
      CreateMap<ToDoTask, TaskDto>();
    }
  }
}