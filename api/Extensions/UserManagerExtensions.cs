using api.DAL;
using api.Data;
using Microsoft.AspNetCore.Identity;

namespace api.Extensions
{
  public static class UserManagerExtensions
  {
    public static async Task<IdentityResult> CreateAppUser(
      this UserManager<AppUser> userManager,
      GenericRepository<ToDoList> listRepository,
      AppUser user,
      string password)
    {
      ToDoList list = new();
      list.Title = "Los Pollos Hermanos";
      list.Description = "Say my name.";
      list.Tasks = new List<ToDoTask>{
        new(){
          Title = "Cook",
          Description = "Can it be more pure?",
          IsCompleted = true,
          Index = 1
        },
        new(){
          Title = "Sell on cartels teritory",
          Description = "Who cares about Don Eladio?",
          IsCompleted = true,
          Index = 2
        },
        new(){
          Title = "Make an offer to Don Eladio",
          Description = "Our chicken is the best.",
          IsCompleted = true,
          Index = 3
        },
        new(){
          Title = "More cook",
          Description = "Will me and Gus be happy now?",
          IsCompleted = false,
          Index = 4
        }
      };
      listRepository.Insert(list);
      user.Lists = new List<ToDoList> { list };
      var result = await userManager.CreateAsync(user, password);
      return result;
    }
  }
}