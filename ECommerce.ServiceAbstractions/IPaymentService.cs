using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstractions
{
    public interface IPaymentService
    {
        Task<BasketDTO> CreateOrUpdatePaymentIntnetAsync(string BasketId);
    }
}
