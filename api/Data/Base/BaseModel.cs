using System.ComponentModel.DataAnnotations;

namespace api.Data.Base
{
  public class BaseModel
  {
    [Key]
    public string Id { get; set; }
  }
}