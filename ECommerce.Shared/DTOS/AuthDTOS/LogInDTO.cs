using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.AuthDTOS
{
    public record LogInDTO ([EmailAddress]string Email , string Password);
}
