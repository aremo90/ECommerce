using ECommerce.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domin.Contract
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IGenericRepository<TEntity, TKey> GetRepositoryAsync<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
