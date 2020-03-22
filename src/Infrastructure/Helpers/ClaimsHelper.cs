using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Dapper;

namespace PenMail.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {

        private static string GetClaim(this IEnumerable<Claim> claims,string parameter)
        {
            var claim = claims.FirstOrDefault(h => h.Type == parameter);
            if (!string.IsNullOrEmpty(claim.Value))
                return claim.Value;
            else
                return string.Empty;
        }

        public static long ToUserId(this IEnumerable<Claim> claims)
        {            
            return Convert.ToInt64(claims.GetClaim("ID")); 
        }

  

        public static bool IsAdmin(this IEnumerable<Claim> claims)
        {            
            return Convert.ToBoolean(claims.GetClaim("IsAdmin")); 
        }

       

        
    }
}
