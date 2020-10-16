using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly BorrowedGamesContext _context;

        internal DbSet<T> dbSet;
        public BaseRepository()
        {
        }
        public BaseRepository(BorrowedGamesContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task<T> Create(T newEntity)
        {
            await dbSet.AddAsync(newEntity);
            return newEntity;
        }

        public virtual T Delete(T deleteEntity)
        {
            if (_context.Entry(deleteEntity).State == EntityState.Detached)
            {
                dbSet.Attach(deleteEntity);
            }
            dbSet.Remove(deleteEntity);
            return deleteEntity;
        }

        public async Task<T> Delete(int entityId)
        {
            var entity = await dbSet.FindAsync(entityId);
            if(entity != null)
                Delete(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> Get()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual T Update(T updateEntity)
        {
            dbSet.Attach(updateEntity);
            _context.Entry(updateEntity).State = EntityState.Modified;
            return updateEntity;
        }
    }
}
