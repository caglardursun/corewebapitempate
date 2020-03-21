using API.Contracts;
using API.Data;
using API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Contracts
{
    public interface IGeneralUserManager :  IRepository<GeneralUser>
    {
        Task<GeneralUser> GetGeneralUserByUserName(string username);
        Task<(IEnumerable<GeneralUser> GeneralUsers, Pagination Pagination)> SearchGeneralUsersAsync(UrlQuerySearchParameters urlQueryParameters);
    }
}
