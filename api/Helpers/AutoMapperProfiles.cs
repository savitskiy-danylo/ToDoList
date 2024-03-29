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
      CreateMap<ToDoList, ListDto>().AfterMap(
        (list, dto) =>
        {
          list.Tasks.OrderBy(task => task.Index);
        }
      );
      CreateMap<ListDto, ToDoList>()
        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(x => x.Tasks, opt => opt.MapFrom(src => src.Tasks));

      CreateMap<ToDoTask, TaskDto>();
      CreateMap<TaskDto, ToDoTask>().ForMember(x => x.List, opt => opt.Ignore());

      CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    }
  }
}