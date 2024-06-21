using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposites.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Repository.Abstractions;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork
{
    public class UnitOfWorkBase<TDbContext, TPk> : IGenericUnitOfWork<TDbContext, TPk>
        where TDbContext : DbContext, IDisposable
        where TPk:struct
    {
        public TDbContext DbContext { get; }

        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWorkBase(TDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepositoryForKeyedEntity<TEntity, TPk> GetRepository<TEntity>() where TEntity : class, IEntityKeyed<TPk>
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
                _repositories[type] = new RepositoryForKeyedEntity<TEntity, TPk>(DbContext);

            return (IRepositoryForKeyedEntity<TEntity, TPk>)_repositories[type];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        #region Disposals

        public void Dispose()
        {
            DbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the context only once if not disposed
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) DbContext?.Dispose();
            _disposed = true;
        }

        #endregion

    }
}