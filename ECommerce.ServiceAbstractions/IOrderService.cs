using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstractions
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO , string email);

        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveMethodsAsync();
        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id , string Email);

    }
}
