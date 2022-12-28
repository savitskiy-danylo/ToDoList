using api.Data.Base;
using Microsoft.AspNetCore.Identity;

namespace api.Data
{
  public class AppUser : IdentityUser, IBaseModel
  {
    public IEnumerable<ToDoList> Lists { get; set; }
  }
}