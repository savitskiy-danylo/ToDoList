namespace api.DTO
{
  public class ListDto
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Discription { get; set; }
    public IEnumerable<TaskDto> Tasks { get; set; }
  }
}