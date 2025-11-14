using ECommerce.Domin.Contract;
using ECommerce.Domin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data
{
    public class SpecificationEvaluator
    {
        public static IQueryable<TEntity>  CreateQuery<TEntity , TKey>(IQueryable<TEntity> inputQuery  ,
            ISpecification<TEntity , TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var Query = inputQuery;
            if (specification is not null)
            {
                //Where
                if (specification.WhereExpression is not null)
                {
                    Query = Query.Where(specification.WhereExpression);
                }




                //Include
                if (specification.IncludeExpresstion is not null && specification.IncludeExpresstion.Any())
                {
                    foreach (var includeExpression in specification.IncludeExpresstion)
                    {
                        Query = Query.Include(includeExpression);
                    }
                }

                //OrderBy
                if (specification.OrderBy is not null)
                {
                    Query = Query.OrderBy(specification.OrderBy);
                }
                if (specification.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specification.OrderByDescending);
                }

                //Pagination
                if (specification.IsPaginated)
                {
                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }

            }
            return Query;
        }
    }
}
