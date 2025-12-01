using ECommerce.ServiceAbstractions;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Contollers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Result = await _orderService.CreateOrderAsync(orderDTO, GetUserEmail());
            return HandleRequest(Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetAllOrders()
        {
            var Orders = await _orderService.GetAllOrdersAsync(GetUserEmail());
            return HandleRequest(Orders);
        }

        //Get Order By Id
        [Authorize]
        [HttpGet("OrderId")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid Id)
        {
            var Order = await _orderService.GetOrderByIdAsync(Id , GetUserEmail());
            return HandleRequest(Order);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethod()
        {
            var Result = await _orderService.GetDeliveMethodsAsync();
            return HandleRequest(Result);
        }

    }
}
