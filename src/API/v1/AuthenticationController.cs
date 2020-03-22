using API.Contracts;
using API.Data;
using API.DTO.Request;
using API.DTO.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;

namespace API.API.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController
    {
        private ILogger<AuthenticationController> _logger;
        private IMapper _mapper;
        private IApiAuthenticationService _authenticationService;

        public AuthenticationController(
         IApiAuthenticationService authenticationService,
         IMapper mapper,
         ILogger<AuthenticationController> logger
         )
        {
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;

            
            
        }


        /// <summary>
        /// Kayıt sistemi Login servisi
        /// </summary>
        /// <param name="user">
        /// <see cref="ApiUserRequest"/> UserName ve Password alanları zorunlu
        /// </param>
        /// <returns>
        /// <see cref="ApiResponse"/>
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse> Post([FromBody] ApiUserRequest user)
        {
            var mapped = _mapper.Map<ApiUser>(user);

            

            var data = await _authenticationService.Authenticate(mapped);
            if (data == null)
            {
                _logger.LogWarning($@"No records Found with UserName : {user.UserName}");
                throw new Exception($@"No records Found with UserName : {user.UserName}");
            }
            //if(!data.IsAdmin)
            //    throw new Exception($@"User Is Not An Admin UserName : {user.UserName}");

            //cast to api response and return it                
            return _mapper.Map<ApiResponse>(data);

        }

    }
}
