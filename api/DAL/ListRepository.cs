using api.Data;

namespace api.DAL
{
  public class ListRepository : GenericRepository<ToDoList>
  {
    public ListRepository(DataContext context) : base(context)
    {
    }
  }
}