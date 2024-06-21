using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.Repo.Generic
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context) : base(context)
        {
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Add(params T[] entities)
        {
            DbSet.AddRange(entities);
        }


        public void Add(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }


        public void Delete(T entity)
        {
            var existing = DbSet.Find(entity);
            if (existing != null) DbSet.Remove(existing);
        }


        public void Delete(object id)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                DbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = DbSet.Find(id);
                if (entity != null) Delete(entity);
            }
        }

        public void Delete(params T[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }


        [Obsolete("Method is replaced by GetList")]
        public IEnumerable<T> Get()
        {
            return DbSet.AsEnumerable();
        }

        [Obsolete("Method is replaced by GetList")]
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsEnumerable();
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Update(params T[] entities)
        {
            DbSet.UpdateRange(entities);
        }


        public void Update(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}