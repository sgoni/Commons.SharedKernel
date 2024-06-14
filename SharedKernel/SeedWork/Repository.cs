using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.SeedWork
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        protected readonly DbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return (IUnitOfWork)_context;
            }
        }

        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            if (context != null)
            {
                _dbSet = _context.Set<TEntity>();
            }
        }

        public IQueryable<TEntity> GetAll(bool asNoTracking = true)
        {
            if (asNoTracking)
                return _context.Set<TEntity>().AsNoTracking();
            else
                return _context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetAllBySpec(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
        {
            if (asNoTracking)
                return _context.Set<TEntity>().Where(predicate).AsNoTracking();
            else
                return _context.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
        {
            return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        public async Task<TEntity> GetBySpecAsync<Spec>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<ICollection<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().Where(predicate).CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AnyAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }

            return queryable;
        }

        public void AddEntity(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void RemoveEntity(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void UpdateEntity(TEntity entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
        }
    }
}
