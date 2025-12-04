using ECommerce.Domin.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification
{
    public class OrderWithPaymentIntentSpec : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentSpec(string IntentId) : base(O => O.PaymentIntentId == IntentId)
        {

        }
    }
}
