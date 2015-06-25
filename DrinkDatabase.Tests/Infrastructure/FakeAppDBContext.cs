using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.AdamExtension;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace DrinkDatabase.Tests.Infrastructure
{
    class FakeAppDBContext : IAppDBContext
    {
        private Dictionary<Type, object> Set = new Dictionary<Type, object>();

        public IQueryable<T> Query<T>() where T : class
        {
            if(!Set.ContainsKey(typeof(T)))
            {
                Set[typeof(T)] = new List<T>();
            }
            return (Set[typeof(T)] as List<T>).AsQueryable<T>();
        }
        public DbEntityEntry Entry(object entity)
        {
            return entity as DbEntityEntry;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.Run<int>(() => { return 1; });
        }

        public void Add<T>(T target) where T : class
        {
            List<T> thisList;
            if(!Set.ContainsKey(typeof(T)))
            {
                thisList = new List<T>();
                Set[typeof(T)] = thisList;
            }
            thisList = Set[typeof(T)] as List<T>;
            thisList.Add(target);
        }

        public void Remove<T>(T target) where T : class
        {
            List<T> thisList = Set[typeof(T)] as List<T>;
            thisList.Remove(target);
        }
        public T Find<T>(params object[] keyValues) where T : class
        {
            var thisList = Query<T>().ToList();
            foreach (var candidate in thisList)
            {
                var info = candidate.GetType().GetProperties();
                foreach (var i in info)
                {
                    foreach (var attr in i.GetCustomAttributes())
                    {
                        if(attr is KeyAttribute)
                        {
                            var thisKey = i.GetValue(candidate);
                            if(keyValues.Contains(thisKey))
                            {
                                return candidate;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            return Task.Run<T>(() => { return Find<T>(keyValues); });
        }

        public void Dispose()
        {
            Set = null;
        }
    }
}
