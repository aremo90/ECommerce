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
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product is null)
                    return NotFound($"No Proudcut with Id: {id} Found!");
                return Ok(product);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentException:
                        return BadRequest(ex.Message);
                    case InvalidOperationException:
                        return BadRequest("Invalid Operation Occurred.");
                    case OutOfMemoryException:
                        return StatusCode(503, "Service Unavaliable ! Try Again Later.");
                    default:
                        return StatusCode(500, "Internal Server Error Occurred.");

                }
            }
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
