using ECommerce.ServiceAbstractions;
using ECommerce.Shared.DTOS.BasketDTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Contollers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasketById(string id)
        {
            var basket = await _basketService.GetBasketAsync(id);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketDto)
        {
            var Basket = await _basketService.CreateOrUpdateAsync(basketDto);
            return Ok(Basket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}
