using api.Data;

namespace api.DAL
{
  public class UnitOfWork : IDisposable
  {
    private readonly DataContext _context;

    private GenericRepository<ToDoTask> _taskRepository;
    public GenericRepository<ToDoTask> TaskRepository
    {
      get
      {
        if (_taskRepository == null) _taskRepository = new(_context);
        return _taskRepository;
      }
    }

    private GenericRepository<ToDoList> _listRepository;
    public GenericRepository<ToDoList> ListRepository
    {
      get
      {
        if (_listRepository == null) _listRepository = new(_context);
        return _listRepository;
      }
    }

    private GenericRepository<AppUser> _userRepository;
    public GenericRepository<AppUser> UserRepository
    {
      get
      {
        if (_userRepository == null) _userRepository = new(_context);
        return _userRepository;
      }
    }


    public UnitOfWork(DataContext context)
    {
      _context = context;

    }

    private bool disposed = false;
    public virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }
      disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}