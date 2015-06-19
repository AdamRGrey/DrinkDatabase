using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.AdamExtension
{
    /// <summary>
    /// Separate the controller from the database. This way you can write tests for controllers without having to connect to the database.
    /// </summary>
    public interface IAppDBContext : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        //stuff the default Application DB contexts already have
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);

        //Convenience methods

        void Add<T>(T target) where T : class;
        void Remove<T>(T target) where T : class;
        T Find<T>(int id) where T : class;
        Task<T> FindAsync<T>(int id) where T : class;
    }
}

/*  default implementations
         
    /// <summary>
    /// Set<<typeparamref name="T"/>>().Add(<paramref name="target"/>)
    /// </summary>
    public void Add<T>(T target) where T : class
    {
        Set<T>().Add(target);
    }
    /// <summary>
    /// Set<<typeparamref name="T"/>>().Remove(<paramref name="target"/>)
    /// </summary>
    public void Remove<T>(T target) where T : class
    {
        Set<T>().Remove(target);
    }
    /// <summary>
    /// Set<<typeparamref name="T"/>>().Find(<paramref name="id"/>)
    /// </summary>
    public T Find<T>(int id) where T : class
    {
        return Set<T>().Find(id);
    }
    /// <summary>
    /// Set<<typeparamref name="T"/>>().FindAsync(<paramref name="id"/>)
    /// </summary>
    public Task<T> FindAsync<T>(int id) where T : class
    {
        return Set<T>().FindAsync(id);
    }
    */