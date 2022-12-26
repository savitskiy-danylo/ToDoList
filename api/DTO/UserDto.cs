using api.Data;

namespace api.DTO
{
  public class UserDto
  {
    public string UserName { get; set; }
    public string Token { get; set; }
    public IEnumerable<ToDoList> Lists { get; set; }
  }
}