namespace api.DTO
{
  public class ListDto
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public IEnumerable<TaskDto> Tasks { get; set; }

  }
}