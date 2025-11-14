using AutoMapper;
using ECommerce.Domin.Contract;
using ECommerce.Domin.Models.ProudctModule;
using ECommerce.Service.Specification;
using ECommerce.ServiceAbstractions;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepositoryAsync<ProductBrand ,int>().GetAllAsync();
            return  _mapper.Map<IEnumerable<BrandDTO>>(Brands);
        }
        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var types = await  _unitOfWork.GetRepositoryAsync<ProductType ,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(types);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParam)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(queryParam);
            var products = await _unitOfWork.GetRepositoryAsync<Product ,int>().GetAllAsync(Spec);
            var DataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            #region Count Data
            var CountOfReturnedData = DataToReturn.Count();
            var CountSpec = new ProductCountSpecification(queryParam);
            var TotalItems = await _unitOfWork.GetRepositoryAsync<Product ,int>().CountAsync(CountSpec);
            #endregion
            return new PaginatedResult<ProductDTO>(queryParam.PageIndex , CountOfReturnedData, TotalItems, DataToReturn);
        }


        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid product ID.");
            }
            var Prodcut = await _unitOfWork.GetRepositoryAsync<Product ,int>().GetByIdAsync(Spec);
            return _mapper.Map<ProductDTO>(Prodcut);
        }
    }
}
