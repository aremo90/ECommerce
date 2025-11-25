using ECommerce.Presentation.Attributes;
using ECommerce.ServiceAbstractions;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Contollers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParams queryParam)
        {
            try
            {

                var products = await _productService.GetAllProductsAsync(queryParam);
                return Ok(products);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return HandleRequest<ProductDTO>(result);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }
        [HttpGet("type")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllType()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }


    }
}
