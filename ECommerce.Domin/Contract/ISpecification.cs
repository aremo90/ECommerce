using ECommerce.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domin.Contract
{
    public interface ISpecification<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        //Inclued Expressions
        public ICollection<Expression<Func<TEntity , object>>> IncludeExpresstion { get; }
        //Where Expressions
        public Expression<Func<TEntity, bool>> WhereExpression { get; }

        //OrderBy Expressions
        public Expression<Func<TEntity, object>>? OrderBy { get; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; }

        //Pagination
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated{ get; set; }
    }
}
