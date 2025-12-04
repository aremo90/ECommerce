using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product , int>
    {
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams) : base(ProductSpecHelper.GetProductFilterExpression(queryParams))
        {
            AddInclude(p => p.ProductBrands);
            AddInclude(p => p.ProductTypes);

            switch (queryParams.Sort)
            {
                case SortingEnum.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case SortingEnum.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case SortingEnum.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case SortingEnum.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrands);
            AddInclude(p => p.ProductTypes);
        }
    }
}
