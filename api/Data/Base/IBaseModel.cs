using System.ComponentModel.DataAnnotations;

namespace api.Data.Base
{
  public interface IBaseModel
  {
    [Key]
    public string Id { get; set; }
  }
}