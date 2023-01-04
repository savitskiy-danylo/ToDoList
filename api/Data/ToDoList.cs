using System.ComponentModel.DataAnnotations.Schema;
using api.Data.Base;

namespace api.Data
{
  public class ToDoList : BaseModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public IEnumerable<ToDoTask> Tasks { get; set; }
  }
}