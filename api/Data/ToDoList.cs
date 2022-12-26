using api.Data.Base;

namespace api.Data
{
  public class ToDoList : BaseModel
  {
    public string Title { get; set; }
    public string Discription { get; set; }
    public AppUser User { get; set; }
    public IEnumerable<ToDoTask> Tasks { get; set; }
  }
}