using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification
{
    public class ProductCountSpecification : BaseSpecification<Product , int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams) : base(ProductSpecHelper.GetProductFilterExpression(queryParams))
        {
            
        }
    }
}
