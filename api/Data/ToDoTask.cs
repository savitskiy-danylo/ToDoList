using api.Data.Base;

namespace api.Data
{
  public class ToDoTask : BaseModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiryDate { get; set; }
    public ToDoList List { get; set; }
  }
}