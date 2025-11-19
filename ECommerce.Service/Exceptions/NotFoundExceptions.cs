using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Exceptions
{
    public abstract class NotFoundExceptions(string message) : Exception(message)
    {
    }

    public class ProductNotFoundException(int id) : NotFoundExceptions($"Product with ID {id} not found.")
    {
    }
    public class BasketNotFoundException : NotFoundExceptions
    {
        public BasketNotFoundException(string id) : base($"Basket with ID {id} not found.")
        {
            
        }
    }
}
