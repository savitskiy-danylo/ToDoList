using api.Data.Base;

namespace api.Data
{
  public class ToDoTask : BaseModel
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }
    public int Index { get; set; }
    public ToDoList List { get; set; }
    public string ListId { get; set; }
  }
}