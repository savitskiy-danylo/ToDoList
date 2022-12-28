using System.ComponentModel.DataAnnotations;

namespace api.Data.Base
{
  public class BaseModel : IBaseModel
  {
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
  }
}