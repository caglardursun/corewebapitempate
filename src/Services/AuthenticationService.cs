using API.Contracts;
using API.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Data.Entity;

namespace API.Services
{
    public class ApiAuthenticationService : IApiAuthenticationService
    {
        private ILogger<ApiAuthenticationService> _logger;

        private IConfiguration _config;
        private IPasswordHashService _passwordHashService;

        private IGeneralUserManager _generalUserManager;

        

        public ApiAuthenticationService(ILogger<ApiAuthenticationService> logger
            , IGeneralUserManager generalUserManager
            , IPasswordHashService passwordHashService 
            , IConfiguration config)
        {
            _logger = logger;            
            _config = config;
            _passwordHashService = passwordHashService;
            _generalUserManager = generalUserManager;
        }


        public async Task<GeneralUser> Authenticate(ApiUser user)
        {
            //Bulk admin insert 

            if ((await _generalUserManager.GetAllAsync()).Any() == false)
            {
                //Create default penadmin user                                 
                
            }

            var _user = await _generalUserManager.GetGeneralUserByUserName(user.UserName);

            if (_user == null)
                throw new SecurityException($@"No record found with this username {user.UserName}");


            //this is the code when you implimented hashed password enties in customer table
            if (_passwordHashService.VerifyHashedPassword(_user.Password, user.Password) == PasswordVerificationResult.Failed)
                throw new SecurityException("Username and password didn't match");

            
            // authentication successful so generate jwt token
            //if it's already exists use existing one


            IdentityModelEventSource.ShowPII = true;

            var tokenHandler = new JwtSecurityTokenHandler();
            // "TimeoutDays" default 365
            var key = Encoding.ASCII.GetBytes(_config["Self:Secret"]);

            var claims = GetGeneralUserClaims(_user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                //    new Claim[]
                //{
                //    new Claim(ClaimTypes.Name, _user.id.ToString())
                //}
                claims
                ),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["Self:TimeoutDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)                

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //_user. = tokenHandler.WriteToken(token);
            //_user. = tokenDescriptor.Expires;
            //check is valid ...
            //if (!await _generalUserManager.UpdateAsync(_user))
            //    throw new SecurityException("Username and Password could not match. Doesn't have admin right");
            _user.Token = tokenHandler.WriteToken(token);
            if(await _generalUserManager.UpdateAsync(_user))
            {
                //token update
            }

            // configure DI for application services
        
            return _user;

        }

        private IEnumerable<Claim> GetGeneralUserClaims(GeneralUser user)
        {
            IEnumerable<Claim> claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, (user.EMail == null) ? "": user.EMail),                    
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim("Id", user.ID.ToString()),
                    //new Claim("email", (user.email == null) ? "": user.email),
                    //new Claim("gsm", (user.gsm == null)? "": user.gsm),
                    new Claim("IsAdmin", user.IsAdmin.ToString()),                    
                    new Claim("IsManager", user.IsManager.ToString())                    
                    };

            return claims;
        }

    }
}
