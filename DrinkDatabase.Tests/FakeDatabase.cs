using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.AdamExtension
{
    class FakeDatabase : IAppDBContext
    {
        public void Dispose() { }

        public Task<int> SaveChangesAsync()
        {
            return null;
        }
        public DbEntityEntry Entry(object entity)
        {
            return entity as DbEntityEntry;
        }
        public IQueryable<T> Query<T>() where T : class
        {
            return Set[typeof(T)] as IQueryable<T>;
        }

        /// <summary>
        /// in memory data-store
        /// </summary>
        private Dictionary<Type, object> Set = new Dictionary<Type, object>();
        /// <summary>
        /// Set<<typeparamref name="T"/>>().Add(<paramref name="target"/>)
        /// </summary>
        public void Add<T>(T target) where T : class
        {
            List<T> thisList = Set[typeof(T)] as List<T>;
            if(thisList == null)
            {
                thisList = new List<T>();
                Set[typeof(T)] = thisList;
            }
            thisList.Add(target);
        }
        /// <summary>
        /// Set<<typeparamref name="T"/>>().Remove(<paramref name="target"/>)
        /// </summary>
        public void Remove<T>(T target) where T : class
        {
            List<T> thisList = Set[typeof(T)] as List<T>;
            thisList.Remove(target);
        }
        /// <summary>
        /// Set<<typeparamref name="T"/>>().Find(<paramref name="id"/>)
        /// </summary>
        public T Find<T>(params object[] keyValues) where T : class
        {
            List<T> queriedList = new List<T>();
            if (Entry(queriedList.First()) == null)
                return null;
            List<T> retList = new List<T>();
            foreach (var key in keyValues)
            {
                //retList.Add(queriedList.First((i) => Entry(i).id == key));
                foreach (var entry in queriedList)
                {
                    var entity = Entry(entry);
                    
                }
            }
            return null;
        }
        /// <summary>
        /// asynchronously call Find
        /// </summary>
        public Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            return Task.Run(() =>
            {
                return Find<T>(keyValues);
            });
        }
    }
}
