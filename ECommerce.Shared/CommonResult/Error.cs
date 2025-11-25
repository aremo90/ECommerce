using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Error
    {
        public string Code { get;}
        public string Description { get;}
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public static Error Failure(string code, string description)
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code, string description)
        {
            return new Error(code, description, ErrorType.validation);
        }
        public static Error NotFound(string code, string description)
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        public static Error UnAuthorized(string code, string description)
        {
            return new Error(code, description, ErrorType.unAuthorized);
        }
        public static Error Forbidden(string code, string description)
        {
            return new Error(code, description, ErrorType.forbidden);
        }
        public static Error InvalidCredentials(string code, string description)
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }



    }
}
