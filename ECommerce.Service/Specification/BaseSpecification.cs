using ECommerce.Domin.Contract;
using ECommerce.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification
{
    public class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region Include Expressions

        public ICollection<Expression<Func<TEntity, object>>> IncludeExpresstion { get; } = [];


        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpresstion.Add(includeExpression);
        }
        #endregion

        #region Where

        public Expression<Func<TEntity, bool>> WhereExpression { get; }



        public BaseSpecification(Expression<Func<TEntity, bool>> WhereExp)
        {
            WhereExpression = WhereExp;
        }

        #endregion

        #region OrderBy

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

       
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        #endregion

        #region 
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; set; }

        protected void ApplyPagination( int pageSize , int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

        #endregion
    }
}
