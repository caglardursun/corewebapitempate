using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace API.DTO.Request
{
    /// <summary>    
    /// <see cref="UserName"/> ve <see cref="Password"/> zorunlu
    /// </summary>
    public class ApiUserRequest
    {        
        public string UserName { get; set; }
        public string Password { get; set; }                        
    }

    public class ApiUserRequestValidator : AbstractValidator<ApiUserRequest>
    {
        public ApiUserRequestValidator()
        {
            RuleFor(o=>o.Password)
                .NotNull()
                .NotEmpty();

            RuleFor(o => o.UserName)
                .NotNull()
                .NotEmpty();

        }

    }
}
