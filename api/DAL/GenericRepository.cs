using System.Linq.Expressions;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.DAL
{
  public class GenericRepository<TEntity> where TEntity : class
  {
    public DataContext _context { get; set; }
    public DbSet<TEntity> _dbSet { get; set; }
    public GenericRepository(DataContext context)
    {
      _context = context;
      _dbSet = context.Set<TEntity>();
    }

    public virtual IEnumerable<TEntity> Get(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null,
      string includeProperties = "")
    {
      IQueryable<TEntity> query = _dbSet;
      if (filter != null)
      {
        query = query.Where(filter);
      }

      foreach (var includeProperty in includeProperties.Split
        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(includeProperty);
      }
      if (orderby != null)
      {
        return orderby(query).ToList();
      }
      else
      {
        return query.ToList();
      }
    }

    public virtual TEntity GetById(object id)
    {
      return _dbSet.Find(id);
    }

    public virtual void Insert(TEntity entity)
    {
      _dbSet.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
      _dbSet.Attach(entity);
      _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
      if (_dbSet.Entry(entity).State == EntityState.Detached)
      {
        _dbSet.Attach(entity);
      }
      _dbSet.Remove(entity);
    }

    public virtual void Delete(object id)
    {
      Delete(_dbSet.Find(id));
    }

  }
}