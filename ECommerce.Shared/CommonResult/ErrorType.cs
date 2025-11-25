using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public enum ErrorType
    {
        Failure=0,
        validation=1,
        NotFound= 2,
        unAuthorized= 3,
        forbidden= 4,
        InvalidCredentials= 5,
        
    }
}
