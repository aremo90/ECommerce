using ECommerce.Domin.Contract;
using ECommerce.Domin.Models;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repository
{
    internal class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) //Tkey since id can be of any type {GUID , INT , ETC}
            => await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task AddAsync(TEntity entity)
            => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void DeleteAsync(TEntity entity)
           => _dbContext.Set<TEntity>().Remove(entity);

        public void UpdateAsync(TEntity entity)
            => _dbContext.Set<TEntity>().Update(entity);
    }
}
