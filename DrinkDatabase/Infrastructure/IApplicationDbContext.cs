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
        T Find<T>(params object[] keyValues) where T : class;
        Task<T> FindAsync<T>(params object[] keyValues) where T : class;
    }
}