using Microsoft.AspNetCore.Identity;

namespace api.Data
{
  public class AppUser : IdentityUser
  {
    public IEnumerable<ToDoList> Lists { get; set; }
  }
}