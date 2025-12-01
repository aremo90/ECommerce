using ECommerce.Domin.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string Email) : base(O => O.UserEmail == Email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }

        public OrderSpecification(Guid Id, string Email) : base(O => O.Id == Id 
                                 &&(string.IsNullOrEmpty(Email) || O.UserEmail.ToLower() == Email.ToLower()))
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }
    }
}
