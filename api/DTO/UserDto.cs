using api.Data;

namespace api.DTO
{
  public class UserDto
  {
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
    public IEnumerable<ListDto> Lists { get; set; }
  }
}